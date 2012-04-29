$(function () {

    //Initialize models
    initModels();

    /* Event handlers */

    //Load sudoku
    $(".loadSudoku").live('click', function () {
        //Get the current id from UI
        var sudokuId = $(this).data('sudokuId');

        //Load
        loadSudokuById(sudokuId);
    });

    //New sudoku
    $("#newSudoku").live('click', function () {

        $.ajax({
            url: 'api/SudokuApi/newsudoku',
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                201 /*Created*/: function (newSudoku) {

                    //Add the item to the list
                    listModel.SudokuList.push(newSudoku);

                    loadGrids(newSudoku.Size);

                    //Load the sudoku
                    loadSudoku(newSudoku);

                },
                400 /* BadRequest */: function (jqxhr) {

                    /* TODO ?! */

                    //var validationResult = $.parseJSON(jqxhr.responseText);
                    //$.validator.unobtrusive.revalidate(form, validationResult);
                }
            }
        });
    });

    //Reset list
    $("#resetList").live('click', function () {

        $.ajax({
            url: 'api/SudokuApi/reset',
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //Load the app again
                    loadApplication();

                },
                400 /* BadRequest */: function (jqxhr) {

                    /* TODO ?! */

                    //var validationResult = $.parseJSON(jqxhr.responseText);
                    //$.validator.unobtrusive.revalidate(form, validationResult);
                }
            }
        });
    });

    //Toggle ready
    $("#toggleReady").live('click', function () {

        $.ajax({
            url: 'api/SudokuApi/toggleready/' + detailsModel.Sudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //Update UI
                    detailsModel.Sudoku().Ready = !detailsModel.Sudoku().Ready;

                    //Bind
                    //TODO This is necessary? Why existing bindings doesn't work, like in List.push()?
                    ko.applyBindings(detailsModel.Sudoku, document.getElementById("detailsPanel"));

                },
                400 /* BadRequest */: function (jqxhr) {

                    /* TODO ?! */
                    //var validationResult = $.parseJSON(jqxhr.responseText);
                    //$.validator.unobtrusive.revalidate(form, validationResult);
                }
            }
        });
    });

    //Toggle auto solve
    $("#toggleAutoSolve").live('click', function () {

        $.ajax({
            url: 'api/SudokuApi/toggleautosolve/' + detailsModel.Sudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //Update UI
                    detailsModel.Sudoku().AutoSolve = !detailsModel.Sudoku().AutoSolve;

                    if (detailsModel.Sudoku().AutoSolve) {
                        //Load it from server to get the updates
                        loadSudoku(detailsModel.Sudoku());
                    } else {
                        ko.applyBindings(detailsModel.Sudoku, document.getElementById("detailsPanel"));
                    }
                },
                400 /* BadRequest */: function (jqxhr) {

                    /* TODO ?! */
                    //var validationResult = $.parseJSON(jqxhr.responseText);
                    //$.validator.unobtrusive.revalidate(form, validationResult);
                }
            }
        });
    });

    //Solve sudoku
    $("#solve").live('click', function () {

        $.ajax({
            url: 'api/SudokuApi/solve/' + detailsModel.Sudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //Load it from the server
                    loadSudoku(detailsModel.Sudoku());

                },
                400 /* BadRequest */: function (jqxhr) {

                    /* TODO ?! */

                    //var validationResult = $.parseJSON(jqxhr.responseText);
                    //$.validator.unobtrusive.revalidate(form, validationResult);
                }
            }
        });
    });

    //Toggle display options
    $("#toggleDisplayOptions").live('click', function () {

        //Toggle
        detailsModel.Sudoku().DisplayOptions = !detailsModel.Sudoku().DisplayOptions;

        //Update UI
        //How about bindings?
        $('#selectedDisplayOption').text(detailsModel.Sudoku().DisplayOptions ? 'Values' : 'IDs');

        $('.squareId').toggle();
        $('.squareValue').toggle();
    });

    //Square input focus + hover
    $(".squareItem").live({
        mouseenter: function ()
        {
            //Get the id
            var id = $(this).attr('id');

            //This is used in both of the grids, so get the pure id
            id = id.replace('mainGrid_', '').replace('availabilityGrid_', '');

            //Highlight both grids
            $('#mainGrid_' + id).addClass('selected');
            $('#availabilityGrid_' + id).addClass('selected');
        },
        mouseleave: function () {

            //Get the id
            var id = $(this).attr('id');

            //This is used in both of the grids, so get the pure id
            id = id.replace('mainGrid_', '').replace('availabilityGrid_', '');

            //Remove highlight from both grids
            $('#mainGrid_' + id).removeClass('selected');
            $('#availabilityGrid_' + id).removeClass('selected');
        }
    });

    //$(".squareValue").live('focus', function () { $(this).select(); });

    //Keydown
    $(".squareValue").live('keydown', function (event) {

        // Allow only backspace and delete
        if (event.keyCode == 9 || event.keyCode == 46 || event.keyCode == 8) {
            // let it happen, don't do anything
        }
        else {
            // Ensure that it is a number and stop the keypress
            //TODO This is only for Size 9, for Size 4 it wouldn't work?!
            //if (event.keyCode < 49 || (event.keyCode > 57 && event.keyCode < 97) || event.keyCode > 105) {
            if (event.keyCode < 49 || event.keyCode > 57) {
                    event.preventDefault();
            }
            else {

                // Ensure that the value is smaller than sudoku size
                var currentValue = $(this).val();
                var newValue = currentValue + String.fromCharCode(event.keyCode);

                if (newValue > detailsModel.Sudoku().Size)
                    event.preventDefault();
            }
        }

    });

    //Square input change
    $(".squareValue").live('change', function () {

        //Get the elements
        var squareValue = $(this);
        var squareItem = squareValue.closest('div');

        //Get the values
        var squareId = squareValue.data('squareId');
        var number = squareValue.val();
        if (number == '')
            number = 0;

        //addDebugMessage('value changed - squareId: ' + squareId + ' - number: ' + number);

        //Prepare post data
        var squareContainer = { SquareId: squareId, Number: number };
        var json = JSON.stringify(squareContainer);

        $.ajax({
            url: 'api/SudokuApi/updatesquare/' + detailsModel.Sudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            data: json,
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //If it it's "Ready", then it must use "user" class, instead of "initial"
                    //TODO Can we use more general approach - when it's "Ready", then all squares defualt class will be "user"?
                    if (detailsModel.Sudoku().Ready) {
                        squareItem.attr('class', squareItem.attr('class').replace('initial', 'user'));
                    }

                    //If it's "AutoSolve", load it from the server
                    if (detailsModel.Sudoku().AutoSolve) {
                        loadSudoku(detailsModel.Sudoku());
                    }
                    else {
                        loadNumbers(detailsModel.Sudoku().SudokuId);
                        loadPotentials(detailsModel.Sudoku().SudokuId);
                        loadAvailabilities(detailsModel.Sudoku().SudokuId);

                        //addDebugMessage('value changed - server responded - squareId: ' + squareId + ' - number: ' + number);
                    }
                },
                400 /* BadRequest */: function (jqxhr) {
                    var validationResult = $.parseJSON(jqxhr.responseText);

                    //Show message
                    showMessagePanel(validationResult);

                    //Clear the square
                    squareValue.val('');

                    //TODO ?
                    //$.validator.unobtrusive.revalidate(form, validationResult);
                }
            }
        });
    });

    $("#messagePanelClear").live('click', function () {
        hideMessagePanel();
    });

    //Numbers
    $(".numberItem").live({
        mouseenter: function () {

            //Get the value
            var number = $(this).data('number');
            number = number == 'Empty' ? number = '' : number;

            //Highlight itself
            $(this).addClass('selected');

            $.each($(".squareValue[value=" + number + "]").closest('div'), function () {

                //Get the id of the related numbers
                id = $(this).attr('id');

                //This is used in both of the grids, so get the pure id
                id = id.replace('mainGrid_', '').replace('availabilityGrid_', '');

                //Highlight both grids
                $('#mainGrid_' + id).addClass('selected');
                $('#availabilityGrid_' + id).addClass('selected');

                //$(".squareValue[value=" + number + "]").closest('div').addClass('selected');
            });

        }, mouseleave: function () {

            //Get the value
            var number = $(this).data('number');
            number = number == 'Empty' ? number = '' : number;

            //Remove highlight from itself
            $(this).removeClass('selected');

            $.each($(".squareValue[value=" + number + "]").closest('div'), function () {

                //This is used in both of the grids, so get the pure id
                id = $(this).attr('id');

                //This is used in both of the grids, so get the pure id
                id = id.replace('mainGrid_', '').replace('availabilityGrid_', '');

                //Highlight both grids
                $('#mainGrid_' + id).removeClass('selected');
                $('#availabilityGrid_' + id).removeClass('selected');

                //$(".squareValue[value=" + number + "]").closest('div').removeClass('selected');
            });
        }
    });

    //Potentials
    $(".potentialItem").live({
        mouseenter: function () {

            var squareId = $(this).data('squareId');
            var potentialValue = $(this).data('potentialValue');

            //Self highlight
            $(this).addClass('selected');

            //Get the square and highlight
            var squareItem1 = $('#mainGrid_squareItem' + squareId);
            squareItem1.addClass('selected');

            var squareItem2 = $('#availabilityGrid_squareItem' + squareId);
            squareItem2.addClass('selected');

            //Get the value element + show the value - EXISTING VALUES WILL BE LOST!
            var squareValue = $('#squareValue' + squareId);
            squareValue.val(potentialValue);

        }, mouseleave: function () {

            var squareId = $(this).data('squareId');
            var potentialValue = $(this).data('potentialValue');

            //Remove self highlight
            $(this).removeClass('selected');

            //Get the square and remove highlight
            var squareItem1 = $('#mainGrid_squareItem' + squareId);
            squareItem1.removeClass('selected');
            var squareItem2 = $('#availabilityGrid_squareItem' + squareId);
            squareItem2.removeClass('selected');

            //Get the value element + remove the value - EXISTING VALUES WILL BE LOST!
            var squareValue = $('#squareValue' + squareId);
            squareValue.val('');
        }
    });

    //Load the app
    loadApplication();
});

function initModels() {
    listModel = { SudokuList: ko.observableArray([]) };

    detailsModel = { Sudoku: ko.observable(null) };

    mainGridModel = { Groups: ko.observableArray([]) };

    availabilityGridModel = { Groups: ko.observableArray([]) };

    numbersModel = { Numbers: ko.observableArray([]) };

    potentialsModel = { Potentials: ko.observableArray([]) };
}

function loadApplication() {

    //Load sudoku list
    loadSudokuList();

    //Load default (first) sudoku
    var sudokuId = 1;
    $.get('api/SudokuApi/item/' + sudokuId, function (sudoku) {

        //Load grid
        loadGrids(sudoku.Size);

        //Load default (first) sudoku
        loadSudoku(sudoku);

        //Debug
        clearDebugPanel();

    });
}

function loadGrids(size) {

    //Main
    loadMainGrid(size);

    //Availability
    loadAvailabilityGrid(size);

    //Debug
    clearDebugPanel();

}

function loadSudokuList() {

    $.get('api/SudokuApi/list', function (sudokuList) {
        listModel.SudokuList = ko.observableArray([]);
        listModel.SudokuList(sudokuList);
        ko.applyBindings(listModel, document.getElementById("listPanel"));
    });

}

function loadMainGrid(size) {

    mainGridModel.Groups = ko.observableArray([]);

    //Create an array for square type groups
    var groups = new Array();
    for (groupCounter = 1; groupCounter <= size; groupCounter++) {

        //Create group
        group = new Object();
        group.GroupId = groupCounter;

        //Squares of the group
        var squares = new Array();
        for (var squareCounter = 1; squareCounter <= size; squareCounter++) {

            //Create square
            var square = new Object();
            square.SquareId = calculateSquareId(size, groupCounter, squareCounter);;
            square.Number = 0;
            squares.push(square);
        }

        group.Squares = squares;
        groups.push(group);
    }

    mainGridModel.Groups(groups);

    ko.applyBindings(mainGridModel, document.getElementById("mainGridPanel"));

    //Styling of the square groups
    $("div.groupItem:odd > .squareItem").addClass('odd');

}

//function loadAvailabilityGrid_OLD(size) {

//    availabilityGridModel.Groups = ko.observableArray([]);

//    //Create an array for square type groups
//    var groups = new Array();
//    for (groupCounter = 1; groupCounter <= size; groupCounter++) {

//        //Create group
//        group = new Object();
//        group.GroupId = groupCounter;

//        //Squares of the group
//        var squares = new Array();
//        for (var squareCounter = 1; squareCounter <= size; squareCounter++) {

//            //Create square
//            var square = new Object();
//            square.SquareId = calculateSquareId(size, groupCounter, squareCounter);;

//            //Availabilities of the square
//            var availabilities = new Array();
//            for (var availabilityCounter = 1; availabilityCounter <= size; availabilityCounter++) {

//                //Create availability item
//                var availability = new Object();
//                availability.AvailabilityId = square.SquareId + '_' + availabilityCounter;
//                availability.Value = availabilityCounter;
//                availabilities.push(availability);
//            }

//            square.Availabilities = availabilities;
//            squares.push(square);
//        }

//        group.Squares = squares;
//        groups.push(group);
//    }

//    availabilityGridModel.Groups(groups);

//    ko.applyBindings(availabilityGridModel, document.getElementById("availabilityGridPanel"));

//    //Styling of the square groups
//    $("div.groupItem:odd > .squareItem").addClass('odd');

//}

function loadAvailabilityGrid() {

    //Size
    var size = mainGridModel.Groups().length;

    //Groups loop
    for (groupCounter = 0; groupCounter < size; groupCounter++) {

        //Get the group
        var group = mainGridModel.Groups()[groupCounter];

        //Squares loop
        for (var squareCounter = 0; squareCounter < size; squareCounter++) {

            //Get the square
            var square = group.Squares[squareCounter];

            //Availabilities of the square
            var availabilities = new Array();
            for (var availabilityCounter = 0; availabilityCounter < size; availabilityCounter++) {

                //Create availability item
                var availability = new Object();
                availability.AvailabilityId = square.SquareId + '_' + (availabilityCounter + 1);
                availability.Value = availabilityCounter + 1;
                availabilities.push(availability);
            }

            //Assign the availabilities
            square.Availabilities = availabilities;
        }
    }

    //Bindings
    ko.applyBindings(mainGridModel, document.getElementById("availabilityGridPanel"));

    //Styling of the square groups
    $("div.groupItem:odd > .squareItem").addClass('odd');

}

function loadSudokuById(sudokuId) {

    $.get('api/SudokuApi/item/' + sudokuId, function (sudoku) {

        loadGrids(sudoku.Size);

        loadSudoku(sudoku);
    });

}

function loadSudoku(sudoku) {

    //Load sudoku
    detailsModel.Sudoku = ko.observable(null);
    detailsModel.Sudoku(sudoku);
    detailsModel.Sudoku().DisplayOptions = true;
    ko.applyBindings(detailsModel.Sudoku, document.getElementById("detailsPanel"));

    //Load used squares
    loadUsedSquares(sudoku.SudokuId);

    //Load numbers
    loadNumbers(sudoku.SudokuId);

    //Load potentials
    loadPotentials(sudoku.SudokuId);

    //Load availabilities
    loadAvailabilities(sudoku.SudokuId);
}

//Is it possible to retrieve only the changes?
//Is it possible to update mainGridModel? - learn more about knockout!
function loadUsedSquares(sudokuId) {

    $.get('api/SudokuApi/usedsquares/' + sudokuId, function (usedSquareList) {

        $.each(usedSquareList, function () {

            //Square item + square value
            var squareItemId = '#squareItem' + this.SquareId;
            var squareValueId = '#squareValue' + this.SquareId;

            //Set the Number
            $(squareValueId).val(this.Number);

            //Assign type
            switch (this.AssignType) {
                case 0:
                    $(squareItemId).removeClass('user').removeClass('solver').addClass('initial');
                    break;

                case 1:
                    $(squareItemId).removeClass('initial').removeClass('solver').addClass('user');
                    break;

                case 2:
                    $(squareItemId).removeClass('initial').removeClass('user').addClass('solver');
                    break;
            }
        });
    });
}

function loadNumbers(sudokuId) {

    $.get('api/SudokuApi/numbers/' + sudokuId, function (numberList) {

        //Zero value
        numberList[0].Value = 'Empty';

        //Remove "0"
        //TODO Dont retrieve it!!!
        numberList.splice(0, 1);

        /* Old */

        //Assign the result
        //numbersModel.Numbers = ko.observableArray([]);
        //numbersModel.Numbers(numberList);
        //ko.applyBindings(numbersModel, document.getElementById("numbersPanel"));

        /* New - TO BE ABLE TO GROUP THE NUMBERS */

        //Square root of the size
        var sqrtSize = Math.sqrt(numberList.length);

        numbersModel.NumberGroups = ko.observableArray([]);

        var numberGroups = new Array();

        for (groupCounter = 0; groupCounter < sqrtSize; groupCounter++) {

            var group = new Object();
            group.Numbers = numberList.splice(0, sqrtSize);
            numberGroups.push(group);
        };

        numbersModel.NumberGroups(numberGroups);

        ko.applyBindings(numbersModel, document.getElementById("numbersPanel"));

    });
}

function loadPotentials(sudokuId) {

    $.get('api/SudokuApi/potentials/' + sudokuId, function (potentialList) {

        potentialsModel.Potentials = ko.observableArray([]);
        potentialsModel.Potentials(potentialList);
        ko.applyBindings(potentialsModel, document.getElementById("potentialsPanel"));

    });
}

function loadAvailabilities(sudokuId) {

    $.get('api/SudokuApi/availabilities/' + sudokuId, function (availabilityList) {

        $.each(availabilityList, function () {

            //Square item + square value
            var availabilityItemId = '#availabilityItem' + this.SquareId + '_' + this.Number;
            //var squareValueId = '#squareValue' + this.SquareId;

            //Set the Number
            if (this.IsAvailable)
                $(availabilityItemId).removeClass('unavailable');
            else
                $(availabilityItemId).addClass('unavailable');

            ////Assign type
            //switch (this.AssignType) {
            //    case 0:
            //        $(squareItemId).removeClass('user').removeClass('solver').addClass('initial');
            //        break;

            //    case 1:
            //        $(squareItemId).removeClass('initial').removeClass('solver').addClass('user');
            //        break;

            //    case 2:
            //        $(squareItemId).removeClass('initial').removeClass('user').addClass('solver');
            //        break;
            //}

            //availabilityGridModel.ava potentialsModel.Potentials = ko.observableArray([]);
            //potentialsModel.Potentials(potentialList);
            //ko.applyBindings(potentialsModel, document.getElementById("potentialsPanel"));

        });
    });
}

//Calculates the square id by using it's group + square position
//This calculation is necessary just because the normal order of the squares (in Engine) goes horizontally;
//If the order would be compatible with this UI (square group type based), this calculation wouldn't be necessary at all!
//Current order of the squares in Engine;
//1st group: 1-2-3-4-5-6-7-8-9
//2nd group: 10-11-12-13-14-15-16-17-18
//Instead of this;
//1st group: 1-2-3 - 2nd group: 10-11-12
//           4-5-6              13-14-15
//           7-8-9              16-17-18
function calculateSquareId(size, groupCounter, squareCounter) {

    //Square root of the size
    var sqrtSize = Math.sqrt(size);

    //Small vertical modifier (on it's own group level);
    //for square counter / modifier: 1 / 1
    //                               2 / 2
    //                               3 / 3
    //                               4 / 1
    //                               5 / 2
    //                               6 / 3
    //                               7 / 1
    //                               8 / 2
    //                               9 / 3
    var smallVerticalModifier = squareCounter % sqrtSize == 0 ? sqrtSize : squareCounter % sqrtSize;

    //Small horizontal modifier (on it's own group level);
    //for square counter / index / modifier: 1 / 0 / 0
    //                                       2 / 0 / 0
    //                                       3 / 0 / 0
    //                                       4 / 1 / 9
    //                                       5 / 1 / 9
    //                                       6 / 1 / 9
    //                                       7 / 2 / 18
    //                                       8 / 2 / 18
    //                                       9 / 2 / 18
    var smallHorizontalIndex = Math.floor((squareCounter - 1) / sqrtSize);
    var smallHorizontalModifier = (smallHorizontalIndex * size);

    //Big vertical modifier (on grid level);
    //for group counter / index / modifier: 1 / 0 / 0
    //                                      2 / 1 / 3
    //                                      3 / 2 / 6
    //                                      4 / 0 / 0
    //                                      5 / 1 / 3
    //                                      6 / 2 / 6
    //                                      7 / 0 / 0
    //                                      8 / 1 / 3
    //                                      9 / 2 / 6
    var bigVerticalIndex = (groupCounter % sqrtSize == 0 ? sqrtSize : groupCounter % sqrtSize) - 1;
    var bigVerticalModifier = (bigVerticalIndex * sqrtSize);

    //Big horizontal modifier (on grid level);
    //for group counter / index / modifier: 1 / 0 / 0
    //                                      2 / 0 / 0
    //                                      3 / 0 / 0
    //                                      4 / 1 / 27
    //                                      5 / 1 / 27
    //                                      6 / 1 / 27
    //                                      7 / 2 / 54
    //                                      8 / 2 / 54
    //                                      9 / 2 / 54
    var bigHorizontalIndex = Math.floor((groupCounter - 1) / sqrtSize);
    var bigHorizontalModifier = (bigHorizontalIndex * size * sqrtSize);

    //Sum all the modifiers and return
    return (smallVerticalModifier + smallHorizontalModifier + bigVerticalModifier + bigHorizontalModifier);
}

function showMessagePanel(message) {
    $('#messagePanelMessage').html($('#messagePanelMessage').html() + '<br />' + message);
    $('#messagePanel').show();
}

function hideMessagePanel() {
    $('#messagePanel').hide();
    $('#messagePanelMessage').text('');
}

function addDebugMessage(message) {
    $('#debugPanel').html($('#debugPanel').html() + '<br>' + message);
    $('#debugPanel').show();
}

function clearDebugPanel() {
    $('#debugPanel').hide();
    $('#debugPanel').html('');
}
