
/* Event handlers */

$("#todoHeader").live('click', function () {
    $("#todoPanel").slideToggle('normal');
});

$("#messagePanelClear").live('click', function () {
    hideMessagePanel();
});

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

            if (newValue > detailsModel.Size())
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
    var squareId = $(squareItem).attr('id');
    squareId = squareId.replace('mainGrid_squareItem', '');

    var number = squareValue.val();
    if (number == '')
        number = 0;

    //Prepare post data
    var squareContainer = { SquareId: squareId, Number: number };
    var json = JSON.stringify(squareContainer);

    $.ajax({
        url: 'api/SudokuApi/updatesquare/' + detailsModel.SudokuId(),
        type: 'POST',
        data: json,
        contentType: 'application/json; charset=utf-8',
        statusCode: {
            200 /*OK*/: function (data) {

                //doesnt work! todo.....
                $(squareItem).data.AssignType(detailsModel.Ready() ? 1 : 0);

                //If it's "AutoSolve", load it from the server
                if (detailsModel.AutoSolve()) {
                    loadUsedSquares(detailsModel.SudokuId());
                }

                //Load numbers
                loadNumbers(detailsModel.SudokuId());

                //Load potentials
                loadPotentials(detailsModel.SudokuId());

                //Load availabilities
                loadAvailabilities(detailsModel.SudokuId());

            },
            400 /* BadRequest */: function (jqxhr) {
                var validationResult = $.parseJSON(jqxhr.responseText);

                //Show message
                showMessagePanel(validationResult);

                //Clear the square
                squareValue.val('');
            }
        }
    });
});

//Numbers
$(".numberItem").live({
    mouseenter: function () {

        //Get the value
        var number = $(this).data('number');

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

//Models
function ListModel() {
    var self = this;
    self.SudokuList = ko.observableArray([]);
    self.loadSudoku = function (sudoku) {
        loadSudoku(sudoku);
    }
    self.newSudoku = newSudoku;
    self.resetList = resetList;
}

function DetailsModel() {

    var self = this;

    //Id, Size, SquaresLeft
    self.SudokuId = ko.observable(0);
    self.Size = ko.observable(0);
    self.TotalSize = ko.computed(function () { return self.Size() * self.Size(); });
    self.SquareRootofSize = ko.computed(function () { return Math.sqrt(self.Size()); });
    self.SquaresLeft = ko.observable(0);

    //Ready
    self.Ready = ko.observable(false);
    self.ToggleReady = toggleReady;
    self.ReadyFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.Ready().toString());
    });

    //AutoSolve
    self.AutoSolve = ko.observable(false);
    self.ToggleAutoSolve = toggleAutoSolve;
    self.AutoSolveFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.AutoSolve().toString());
    });

    //Solve
    self.Solve = solve;

    //Display Options
    self.DisplayOptions = ko.observable(true);
    self.ToggleDisplayOptions = function () {
        self.DisplayOptions(!self.DisplayOptions());
        $('.squareId').toggle();
        $('.squareValue').toggle();
    }
    self.DisplayOptionsFormatted = ko.computed(function () {
        return self.DisplayOptions() ? 'Values' : 'IDs';
    });
}

function GridModel() {
    var self = this;
    self.Groups = ko.observableArray([]);
}

function NumbersModel() {
    var self = this;
    self.NumberGroups = ko.observableArray([]);
}

function PotentialsModel() {
    var self = this;
    self.Potentials = ko.observableArray([]);
}

//Objects
function Group() {
    var self = this;
    self.GroupId = 0;
    self.Squares = ko.observableArray([]);
}

function Square() {
    var self = this;
    self.SquareId = 0;
    self.Number = ko.observable(0);
    self.NumberFormatted = ko.computed(function () { return self.Number() == 0 ? '' : self.Number().toString() });
    self.AssignType = ko.observable(0);
    self.Availabilities = ko.observableArray([]);
    self.IsSelected = ko.observable(false);
    self.ToggleSelect = function (data, event) { self.IsSelected(event.type == 'mouseenter'); }
}

function NumberXXX() {
    var self = this;
    self.Value = 0;
    self.Count = ko.observable(0);
    //self.IsSelected = ko.observable(false);
    //self.ToggleSelect = function (data, event) { self.IsSelected(event.type == 'mouseenter'); }
}

function Potential() {
    var self = this;
    self.SquareId = 0;
    self.PotentialValue = 0;
    self.PotentialType = 0;
    self.IsSelected = ko.observable(false);
    self.ToggleSelect = function (data, event) {

        //Toggle select itself
        self.IsSelected(event.type == 'mouseenter');

        //Find & toggle select related square as well
        var relatedSquare = findSquareBySquareId(data.SquareId);
        relatedSquare.Number(event.type == 'mouseenter' ? self.PotentialValue : '');
        relatedSquare.ToggleSelect(null, event);
        //relatedSquare.AssignType(event.type == 'mouseenter' ? 1 : 0);
    }
}

function Availability() {
    var self = this;
    self.Value = 0; //Number instead of Value?
    self.IsAvailable = ko.observable(true);
}

function loadGrids(size) {

    //addDebugMessage("3.6");

    //Main
    loadMainGrid(size);

    //addDebugMessage("3.7");

    //Availability
    loadAvailabilityGrid(size);

    //addDebugMessage("3.8");

}

function loadSudokuList() {

    $.get('api/SudokuApi/list', function (sudokuList) {

        listModel.SudokuList([]);

        //addDebugMessage('1');
        listModel.SudokuList(sudokuList);

        //addDebugMessage('2');
        loadSudoku(listModel.SudokuList()[0]);
    });
}

function loadMainGrid(size) {

    //addDebugMessage('3.6.1');

    //Create an array for square type groups
    gridModel.Groups([]);
    for (groupCounter = 1; groupCounter <= size; groupCounter++) {

        //Create group
        var group = new Group();
        group.GroupId = groupCounter;

        //addDebugMessage('3.6.1 - groupId: ' + group.GroupId);

        //Squares loop
        for (var squareCounter = 1; squareCounter <= size; squareCounter++) {

            //Create square
            square = new Square();
            square.SquareId = calculateSquareId(size, groupCounter, squareCounter);
            group.Squares.push(square);

            //addDebugMessage('3.6.1 - squareId: ' + square.SquareId);

        }

        gridModel.Groups.push(group);
    }

    //addDebugMessage('3.6.2');

    //Styling of the square groups
    //TODO is it in the correct place?
    $("div.groupItem:odd > .squareItem").addClass('odd');

}

function loadAvailabilityGrid(size) {

    //Groups loop
    for (groupCounter = 0; groupCounter < size; groupCounter++) {

        //Get the group
        var group = gridModel.Groups()[groupCounter];

        //Squares loop
        for (var squareCounter = 0; squareCounter < size; squareCounter++) {

            //Get the square
            var square = group.Squares()[squareCounter];

            //Availability loop
            for (var availabilityCounter = 0; availabilityCounter < size; availabilityCounter++) {

                //Create availability item
                var availability = new Availability();
                availability.Value = availabilityCounter + 1;
                square.Availabilities.push(availability);
            }
        }
    }
}

function loadSudoku(sudoku) {

    //addDebugMessage('3');

    //Load sudoku
    detailsModel.SudokuId(sudoku.SudokuId);
    detailsModel.Size(sudoku.Size);
    detailsModel.Ready(sudoku.Ready);
    detailsModel.AutoSolve(sudoku.AutoSolve);

    //addDebugMessage('3.5');

    //Load grids
    loadGrids(sudoku.Size);

    //addDebugMessage('4');

    //Load used squares
    loadUsedSquares(sudoku.SudokuId);

    //addDebugMessage('5');

    //Load numbers
    loadNumbers(sudoku.SudokuId);

    //addDebugMessage('6');

    //Load potentials
    loadPotentials(sudoku.SudokuId);

    //Load availabilities
    loadAvailabilities(sudoku.SudokuId);

    //Debug
    clearDebugPanel();

    //addDebugMessage('7');

    //Update classes
    //TODO This code is double ?! - repeated in toggleReady
    //var ready = detailsModel.Ready();
    //$(".squareValue[value=]").closest('div').toggleClass('initial', !ready).toggleClass('user', ready);
}

//Is it possible to retrieve only the changes?
function loadUsedSquares(sudokuId) {

    $.get('api/SudokuApi/usedsquares/' + sudokuId, function (usedSquareList) {

        $.each(usedSquareList, function () {

            //Find the square
            var matchedSquare = findSquareBySquareId(this.SquareId);

            //Update
            matchedSquare.Number(this.Number);
            matchedSquare.AssignType(this.AssignType);
        });
    });
}

function loadNumbers(sudokuId) {

    $.get('api/SudokuApi/numbers/' + sudokuId, function (numberList) {

        //Zero value (SquaresLeft on detailsPanel)
        var zeroNumber = numberList.splice(0, 1);
        detailsModel.SquaresLeft(zeroNumber[0].Count);

        //ko.applyBindings(detailsModel.Sudoku, document.getElementById("detailsPanel"));

        // Group the numbers, to be able to display them nicely (see numbersPanel on default.html)

        // Square root of the size
        //var sqrtSize = Math.sqrt(numberList.length);

        //numbersModel.NumberGroups = ko.observableArray([]);

        //var numberGroups = new Array();

        numbersModel.NumberGroups([]);
        var sqrtSize = detailsModel.SquareRootofSize();

        //Group the numbers, to be able to display them nicely (see numbersPanel on default.html)
        for (groupCounter = 0; groupCounter < sqrtSize; groupCounter++) {

            var numberGroup = new Object();
            //group.Numbers = new Array();
            numberGroup.Numbers = new Array();
            //for (numberCounter = 0; numberCounter < sqrtSize; numberCounter++) {
            //var number = new Number();
            //var numberData = numberList.splice(0, 1);
            //number.Value = numberData.Value;
            //number.Count(numberData.Count);
            //group.Numbers.push(number);
            //}

            numberGroup.Numbers = numberList.splice(0, sqrtSize);
            numbersModel.NumberGroups.push(numberGroup);
        };
    });
}

function loadPotentials(sudokuId) {

    $.get('api/SudokuApi/potentials/' + sudokuId, function (potentialList) {

        potentialsModel.Potentials([]);

        $.each(potentialList, function () {

            //Create potential
            var potential = new Potential();
            potential.SquareId = this.SquareId;
            potential.PotentialValue = this.PotentialValue;
            potential.PotentialType = this.PotentialType;
            potentialsModel.Potentials.push(potential);
        });
    });
}

function loadAvailabilities(sudokuId) {

    $.get('api/SudokuApi/availabilities/' + sudokuId, function (availabilityList) {

        $.each(availabilityList, function () {

            //Number
            var number = this.Number;

            //Find the square
            var matchedSquare = findSquareBySquareId(this.SquareId);

            //Find the availability
            var matchedAvailability = ko.utils.arrayFirst(matchedSquare.Availabilities(), function (availability) {
                return availability.Value === number;
            });

            //Update
            matchedAvailability.IsAvailable(this.IsAvailable);

        });
    });
}

function newSudoku() {

    $.ajax({
        url: 'api/SudokuApi/newsudoku',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        statusCode: {
            201 /*Created*/: function (newSudoku) {

                //Add the item to the list
                listModel.SudokuList.push(newSudoku);

                //Load the sudoku
                loadSudoku(newSudoku);

            },
            400 /* BadRequest */: function (jqxhr) {
                var validationResult = $.parseJSON(jqxhr.responseText);

                //Show message
                showMessagePanel(validationResult);
            }
        }
    });
};

function resetList() {

    $.ajax({
        url: 'api/SudokuApi/reset',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        statusCode: {
            200 /*OK*/: function () {

                //Load app again
                loadSudokuList();

            },
            400 /* BadRequest */: function (jqxhr) {
                var validationResult = $.parseJSON(jqxhr.responseText);

                //Show message
                showMessagePanel(validationResult);
            }
        }
    });
}

function toggleReady() {

    $.ajax({
        url: 'api/SudokuApi/toggleready/' + detailsModel.SudokuId(),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        statusCode: {
            200 /*OK*/: function () {

                //Toggle
                detailsModel.Ready(!detailsModel.Ready());

                //UI: Update css classes of the empty squares
                //$(".squareValue[value=]").closest('div').toggleClass('initial', !detailsModel.Ready()).toggleClass('user', detailsModel.Ready());
            },
            400 /* BadRequest */: function (jqxhr) {
                var validationResult = $.parseJSON(jqxhr.responseText);

                //Show message
                showMessagePanel(validationResult);
            }
        }
    });
}

function toggleAutoSolve() {

    $.ajax({
        url: 'api/SudokuApi/toggleautosolve/' + detailsModel.SudokuId(),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        statusCode: {
            200 /*OK*/: function () {

                //Update UI
                detailsModel.AutoSolve(!detailsModel.AutoSolve());

                if (detailsModel.AutoSolve()) {

                    //Load used squares
                    loadUsedSquares(detailsModel.SudokuId());

                    //Load numbers
                    loadNumbers(detailsModel.SudokuId());

                    //Load potentials
                    loadPotentials(detailsModel.SudokuId());
                }
            },
            400 /* BadRequest */: function (jqxhr) {
                var validationResult = $.parseJSON(jqxhr.responseText);

                //Show message
                showMessagePanel(validationResult);
            }
        }
    });
}

function solve() {

    $.ajax({
        url: 'api/SudokuApi/solve/' + detailsModel.SudokuId(),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        statusCode: {
            200 /*OK*/: function () {

                //Load used squares
                loadUsedSquares(detailsModel.SudokuId());

                //Load numbers
                loadNumbers(detailsModel.SudokuId());

                //Load potentials
                loadPotentials(detailsModel.SudokuId());

            },
            400 /* BadRequest */: function (jqxhr) {
                var validationResult = $.parseJSON(jqxhr.responseText);

                //Show message
                showMessagePanel(validationResult);
            }
        }
    });
}

//TODO Use ko.utils.arrayFilter instead?
function findSquareBySquareId(squareId) {

    var matchedSquare = null;

    //Loop through groups
    ko.utils.arrayFirst(gridModel.Groups(), function (group) {

        //Loop through squares
        matchedSquare = ko.utils.arrayFirst(group.Squares(), function (square) {

            //Return if you find the square
            return square.SquareId === squareId;
        });

        //Stop groups loop as well
        return matchedSquare !== null && matchedSquare !== undefined;
    });

    return matchedSquare;
}

//TODO Use ko.utils.arrayFilter instead?
//function findSquaresByNumber(number) {

//    var matchedSquares = new Array();

//    //Loop through groups
//    ko.utils.arrayForEach(gridModel.Groups(), function (group) {

//        //Loop through squares
//        ko.utils.arrayForEach(group.Squares(), function (square) {

//            //If it matches, add to the list
//            if (squareId.Number === number)
//                matchedSquares.push(square);
//        });
//    });

//    return matchedSquares;
//}

//Move to a better place
function capitaliseFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
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
