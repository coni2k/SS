function SudokuViewModel() {
    var self = this;

    //Id, Size, SquaresLeft
    self.SudokuId = ko.observable(0);
    self.Title = ko.observable('');
    self.Description = ko.observable('');
    self.Size = ko.observable(0);
    self.TotalSize = ko.computed(function () { return self.Size() * self.Size(); });
    self.SquareRootofSize = ko.computed(function () { return Math.sqrt(self.Size()); });
    self.SquaresLeft = ko.observable(0);
    self.SudokuList = ko.observableArray([]);

    self.LoadSudoku = function (sudoku) { loadSudoku(self, sudoku); }
    self.NewSudoku = function () { newSudoku(self); }
    self.ResetList = function () { resetList(self); }

    //Ready
    self.Ready = ko.observable(false);
    self.ToggleReady = function () { toggleReady(self); }
    self.ReadyFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.Ready().toString());
    });

    //AutoSolve
    self.AutoSolve = ko.observable(false);
    self.ToggleAutoSolve = function () { toggleAutoSolve(self); }
    self.AutoSolveFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.AutoSolve().toString());
    });

    //Solve
    self.Solve = function () { solve(self); };

    //Groups (squares)
    self.Groups = ko.observableArray([]);
    //TODO initGrid here?

    //Value grid
    self.ValueGrid = new ValueGrid(self.Groups);

    //Availability grid
    self.AvailabilityGrid = new AvailabilityGrid(self.Groups);
    self.ToggleDisplayAvailabilities = function () {
        self.AvailabilityGrid.Visible(!self.AvailabilityGrid.Visible());
    }
    self.DisplayAvailabilitiesFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.AvailabilityGrid.Visible().toString());
    });

    //Availability2 grid
    self.Availability2Grid = new Availability2Grid(self.Groups);
    //self.ToggleDisplayAvailabilities = function () {
    //    self.AvailabilityGrid.Visible(!self.AvailabilityGrid.Visible());
    //}
    //self.DisplayAvailabilitiesFormatted = ko.computed(function () {
    //    return capitaliseFirstLetter(self.AvailabilityGrid.Visible().toString());
    //});

    //Id grid
    self.IdGrid = new IDGrid(self.Groups);
    self.ToggleDisplayIDs = function () {
        self.IdGrid.Visible(!self.IdGrid.Visible());
    }
    self.DisplayIDsFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.IdGrid.Visible().toString());
    });

    //Numbers
    self.NumberGroups = ko.observableArray([]);

    //Potentials
    self.Potentials = ko.observableArray([]);

    //Group number availabilities
    self.GroupNumberAvailabilities = ko.observableArray([]);

    //Square filters
    //a. by Square Id
    self.FilteredSquaresById = function (squareId) {

        var matchedSquare = null;

        //Loop through groups
        ko.utils.arrayFirst(self.Groups(), function (group) {

            //Loop through squares
            matchedSquare = ko.utils.arrayFirst(group.Squares(), function (square) {

                //Return if you find the square
                return square.SquareId === squareId;
            });

            //Stop groups loop as well
            return matchedSquare !== null && matchedSquare !== undefined;
        });

        return matchedSquare;

    };

    //b. by Number
    self.FilteredSquaresByNumber = function (number) {

        var matchedSquares = new Array();

        //Loop through groups
        ko.utils.arrayForEach(self.Groups(), function (group) {

            //Loop through squares
            ko.utils.arrayForEach(group.Squares(), function (square) {

                //If it matches, add to the list
                if (square.Value() === number)
                    matchedSquares.push(square);
            });
        });

        return matchedSquares;
    };
}

function ValueGrid(groups) {
    var self = this;
    self.Groups = ko.observableArray(groups);
    self.DisplayMode = 'value';
    //Visible = always!
}

function AvailabilityGrid(groups) {
    var self = this;
    self.Groups = ko.observableArray(groups);
    self.DisplayMode = 'availability';
    self.Visible = ko.observable(true);
}

function Availability2Grid(groups) {
    var self = this;
    self.Groups = ko.observableArray(groups);
    self.DisplayMode = 'availability2';
    //self.Visible = ko.observable(true);
    self.Visible = true; //ko.observable(true);
}

function IDGrid(groups) {
    var self = this;
    self.Groups = ko.observableArray(groups);
    self.DisplayMode = 'id';
    self.Visible = ko.observable(false);
}

function Group(model) {
    var self = this;
    self.Model = model;
    self.GroupId = ko.observable(0);
    self.Squares = ko.observableArray([]);
    self.IsOdd = ko.computed(function () { return self.GroupId() % 2 === 0; });
}

function Square(group, id) {
    var self = this;
    self.Group = group;
    self.SquareId = id;
    self.Value = ko.observable(0);
    self.ValueFormatted = ko.computed({
        read: function () {
            return self.Value() === 0 ? '' : self.Value();
        },
        write: function (value) {
            self.Value(value === '' ? 0 : value);
        }
    });
    self.OldValue = 0;
    self.IsAvailable = ko.computed(function () { return self.Value() === 0; });
    self.AssignType = ko.observable(0);
    self.Availabilities = ko.observableArray([]);
    self.Availabilities2 = ko.observableArray([]);
    self.IsSelected = ko.observable(false);
    self.IsRelatedSelected = ko.observable(false);
    self.ToggleSelect = function (data, event) {
        //Self selected
        self.IsSelected(event.type == 'mouseenter');

        //Related square selected
        ko.utils.arrayForEach(data.Group.Squares(), function (square) {
            if (square !== data)
                square.IsRelatedSelected(data.IsSelected());
        });
    }

    //Ensure that it is a valid number
    self.Updating = function (data, event) {

        //Hide previous error messages
        hideMessagePanel();

        //TODO This is only for Size 9, for Size 4 it wouldn't work?!
        if (event.keyCode === 8 || //Backspace
            event.keyCode === 9 || //Tab
            event.keyCode === 37 || //Left arrow
            event.keyCode === 39 || //Right arrow
            event.keyCode === 46) //Delete
        {
            //Let it happen
        }
        else if (event.keyCode < 49 || event.keyCode > 57) /* Numbers 1 to 9 - BUT HOW ABOUT NUMPAD? */ {
            event.preventDefault();
        }
        else {

            //Keep the value; this value will be restored in case of a server error
            data.OldValue = data.Value();

            //To be able to do "new value" validation properly, first reset the current value
            data.Value(0);

            //Ensure that the value is smaller than sudoku size
            var newValue = $(event.target).val() + String.fromCharCode(event.keyCode);
            if (newValue > data.Group.Model.Size()) {
                event.preventDefault();
            }
        }

        return true;
    }

    self.Update = function (data, event) {

        //Prepare post data
        var squareContainer = JSON.stringify({ SquareId: data.SquareId, Number: data.Value() });

        //Post; because contentType needs to be set, $.post() couldn't be used
        $.ajax({
            url: serverUrl + 'updatesquare/' + data.Group.Model.SudokuId(),
            type: 'POST',
            data: squareContainer,
            contentType: 'application/json; charset=utf-8'
        }).done(function () {

            //Assign type
            data.AssignType(data.Group.Model.Ready() ? 1 : 0);

            //Load details (if it's AutoSolve, load "used squares" as well
            loadSudokuDetails(data.Group.Model, data.Group.Model.AutoSolve());

        }).fail(function (jqXHR) {
            handleError(jqXHR);

            //Clear the square
            data.Value(data.OldValue);
        });
    }
}

function SudokuNumber(model) {
    var self = this;
    self.Model = model;
    self.Value = 0;
    self.Count = ko.observable(0);
    self.IsSelected = ko.observable(false);
    self.ToggleSelect = function (data, event) {

        //Toggle select itself
        self.IsSelected(event.type == 'mouseenter');

        //Find & toggle select related square as well
        var relatedSquares = data.Model.FilteredSquaresByNumber(data.Value);
        $.each(relatedSquares, function () {
            this.ToggleSelect(this, event);
        });
    }
}

function Potential(model) {
    var self = this;
    self.Model = model;
    self.SquareId = 0;
    self.PotentialValue = 0;
    self.PotentialType = 0;
    self.IsSelected = ko.observable(false);
    self.ToggleSelect = function (data, event) {

        //Toggle select itself
        self.IsSelected(event.type == 'mouseenter');

        //Find & toggle select related square as well
        var relatedSquare = data.Model.FilteredSquaresById(data.SquareId);
        relatedSquare.Value(event.type == 'mouseenter' ? self.PotentialValue : 0);
        relatedSquare.ToggleSelect(relatedSquare, event);
    }
}

function Availability() {
    var self = this;
    self.Value = 0;
    self.IsAvailable = ko.observable(true);
}

function Availability2() {
    var self = this;
    self.Value = 0;
    self.IsAvailable = ko.observable(true);
}

function GroupNumberAvailability() {
    var self = this;
    self.GroupId = 0;
    self.Number = 0;
    self.Count = 0;
}

function loadSudokuList(model) {

    $.getJSON(serverUrl + 'list', function (sudokuList) {

        model.SudokuList([]);
        model.SudokuList(sudokuList);

        loadSudoku(model, model.SudokuList()[0]);

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function loadSudoku(model, sudoku) {

    //Determines whether the a new grid needs to be initialized or the existing one will be refreshed
    var initOrRefreshGrid = model.Size() !== sudoku.Size;

    //Load sudoku
    model.SudokuId(sudoku.SudokuId);
    model.Title(sudoku.Title);
    model.Description(sudoku.Description);
    model.Size(sudoku.Size);
    model.Ready(sudoku.Ready);
    model.AutoSolve(sudoku.AutoSolve);

    //Grid
    if (initOrRefreshGrid)
        initGrid(model);
    else
        refreshGrid(model);

    //Load details
    loadSudokuDetails(model);

    //Clear messages
    hideMessagePanel();
}

function loadSudokuDetails(model, refreshSquares)
{
    //Optional refresh squares, default value is 'true'
    refreshSquares = (typeof refreshSquares === "undefined") ? true : refreshSquares

    //If refreshSquares flag is true, load used squares
    if (refreshSquares) {
        loadUsedSquares(model);
    }

    //Load numbers
    loadNumbers(model);

    //Load potentials
    loadPotentials(model);

    //Load availabilities
    loadAvailabilities(model);

    //Load availabilities
    loadAvailabilities2(model);

    //Load group number availabilities
    loadGroupNumberAvailabilities(model);

}

function initGrid(model) {

    //Create an array for square type groups
    model.Groups([]);
    var size = model.Size();
    for (groupCounter = 1; groupCounter <= size; groupCounter++) {

        //Create group
        var group = new Group(model);
        group.GroupId(groupCounter);

        //Squares loop
        for (var squareCounter = 1; squareCounter <= size; squareCounter++) {

            //Create square
            var squareId = calculateSquareId(size, groupCounter, squareCounter);
            square = new Square(group, squareId);

            //TODO Is it correct way of doing it?
            if (model.Ready())
                square.AssignType(1);

            //Availability loop
            for (var availabilityCounter = 0; availabilityCounter < size; availabilityCounter++) {

                //Create availability item
                var availability = new Availability();
                availability.Value = availabilityCounter + 1;
                square.Availabilities.push(availability);

                //TODO NEW BLOCK!
                //Create availability2 item
                var availability2 = new Availability2();
                availability2.Value = availabilityCounter + 1;
                square.Availabilities2.push(availability2);
            }

            group.Squares.push(square);
        }

        model.Groups.push(group);
    }

    //UI calculations;
    var minWidth = 15;

    var squareWidth = minWidth * model.SquareRootofSize();

    var groupWidth = (squareWidth * model.SquareRootofSize()) + (2 * model.SquareRootofSize()); /* Border */;

    var panelWidth = (groupWidth * model.SquareRootofSize()) + (2 * model.SquareRootofSize()); /* Border */;
    
    $('.squareValue').css('width', squareWidth);
    $('.squareValue').css('height', squareWidth);

    $('.squareAvailabilities').css('width', squareWidth);

    $('.squareId').css('width', squareWidth);
    $('.squareId').css('line-height', squareWidth.toString() + 'px');

    $('.groupItem').css('width', groupWidth);

    $('.gridPanel').css('width', panelWidth);

    $('.numberItem').css('width', squareWidth);
    $('.numberItem').css('height', squareWidth);
    $('.numberItem').css('line-height', squareWidth.toString() + 'px');

    $('.numberGroupItem').css('width', groupWidth);

    $('.numberGroupContainer').css('width', panelWidth);

}

function refreshGrid(model) {

    ko.utils.arrayForEach(model.Groups(), function (group) {
        ko.utils.arrayForEach(group.Squares(), function (square) {
            square.Value(0);
            square.AssignType(model.Ready() ? 1 : 0);
            ko.utils.arrayForEach(square.Availabilities(), function (availability) {
                availability.IsAvailable(true);
            });
        });
    });
}

//Is it possible to retrieve only the changes?
function loadUsedSquares(model) {

    $.getJSON(serverUrl + 'usedsquares/' + model.SudokuId(), function (usedSquareList) {

        $.each(usedSquareList, function () {

            //Find the square
            var matchedSquare = model.FilteredSquaresById(this.SquareId);

            //Update
            matchedSquare.Value(this.Number);
            matchedSquare.AssignType(this.AssignType);
        });

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function loadNumbers(model) {

    $.getJSON(serverUrl + 'numbers/' + model.SudokuId(), function (numberList) {

        //Zero value (SquaresLeft on detailsPanel)
        var zeroNumber = numberList.splice(0, 1);
        model.SquaresLeft(zeroNumber[0].Count);

        //TODO Why cant it be used without assigning to a local?
        var sqrtSize = model.SquareRootofSize();

        //Group the numbers, to be able to display them nicely (see numbersPanel on default.html)
        model.NumberGroups([]);
        for (groupCounter = 0; groupCounter < sqrtSize; groupCounter++) {

            var numberGroup = new Object();
            numberGroup.Numbers = new Array();

            for (numberCounter = 0; numberCounter < sqrtSize; numberCounter++) {

                var numberData = numberList.splice(0, 1);

                var sudokuNumber = new SudokuNumber(model);
                sudokuNumber.Value = numberData[0].Value;
                sudokuNumber.Count(numberData[0].Count);
                numberGroup.Numbers.push(sudokuNumber);
            }

            model.NumberGroups.push(numberGroup);
        };

        //UI calculations;
        var minWidth = 15;

        var squareWidth = minWidth * model.SquareRootofSize();

        var groupWidth = (squareWidth * model.SquareRootofSize()) + (2 * model.SquareRootofSize()); /* Border */;

        var panelWidth = (groupWidth * model.SquareRootofSize()) + (2 * model.SquareRootofSize()); /* Border */;

        $('.numberItem').css('width', squareWidth);
        $('.numberItem').css('height', squareWidth);
        $('.numberItem').css('line-height', squareWidth.toString() + 'px');

        $('.numberGroupItem').css('width', groupWidth);

        $('.numberGroupContainer').css('width', panelWidth);


    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function loadPotentials(model) {

    $.getJSON(serverUrl + 'potentials/' + model.SudokuId(), function (potentialList) {

        model.Potentials([]);

        $.each(potentialList, function () {

            //Create potential
            var potential = new Potential(model);
            potential.SquareId = this.SquareId;
            potential.PotentialValue = this.PotentialValue;
            potential.PotentialType = this.PotentialType;
            model.Potentials.push(potential);
        });
    
    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function loadAvailabilities(model) {

    $.getJSON(serverUrl + 'availabilities/' + model.SudokuId(), function (availabilityList) {

        $.each(availabilityList, function () {

            //Number
            var number = this.Number;

            //Find the square
            var matchedSquare = model.FilteredSquaresById(this.SquareId);

            //Find the availability
            var matchedAvailability = ko.utils.arrayFirst(matchedSquare.Availabilities(), function (availability) {
                return availability.Value === number;
            });

            //Update
            matchedAvailability.IsAvailable(this.IsAvailable);

        });

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function loadAvailabilities2(model) {

    $.getJSON(serverUrl + 'availabilities2/' + model.SudokuId(), function (list) {

        $.each(list, function () {

            //Number
            var number = this.Number;

            //Find the square
            var matchedSquare = model.FilteredSquaresById(this.SquareId);

            //Find the availability
            var matched = ko.utils.arrayFirst(matchedSquare.Availabilities2(), function (availability2) {
                return availability2.Value === number;
            });

            //Update
            matched.IsAvailable(this.IsAvailable);

        });

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function loadGroupNumberAvailabilities(model) {

    $.getJSON(serverUrl + 'groupnumberavailabilities/' + model.SudokuId(), function (list) {

        model.GroupNumberAvailabilities([]);

        $.each(list, function () {

            //Create potential
            var groupNumberAvailability = new GroupNumberAvailability();
            groupNumberAvailability.GroupId = this.GroupId;
            groupNumberAvailability.Number = this.Number;
            groupNumberAvailability.Count = this.Count;
            model.GroupNumberAvailabilities.push(groupNumberAvailability);
        });

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function newSudoku(model) {

    $.post(serverUrl + 'item').done(function (newSudoku)
    {
        //Add the item to the list
        model.SudokuList.push(newSudoku);

        //Load the sudoku
        loadSudoku(model, newSudoku);

    }).fail(function (jqXHR) { handleError(jqXHR); });
};

function resetList(model) {

    $.post(serverUrl + 'reset').done(function () {

        //Load app again
        loadSudokuList(model);

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function toggleReady(model) {

    $.post(serverUrl + 'toggleready/' + model.SudokuId()).done(function () {

        //Toggle
        model.Ready(!model.Ready());

        //Assign Types
        ko.utils.arrayForEach(model.Groups(), function (group) {
            ko.utils.arrayForEach(group.Squares(), function (square) {
                if (square.Value() === 0)
                    square.AssignType(model.Ready() ? 1 : 0);
            });
        });

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function toggleAutoSolve(model) {

    $.post(serverUrl + 'toggleautosolve/' + model.SudokuId()).done(function () {

        //Update UI
        model.AutoSolve(!model.AutoSolve());

        //If autosolve, load details
        if (model.AutoSolve()) { loadSudokuDetails(model); }

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function solve(model) {

    $.post(serverUrl + 'solve/' + model.SudokuId()).done(function () {

        //Load details
        loadSudokuDetails(model);

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

//TODO Should there be generic functions js file?
function capitaliseFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

//Loading panel
$(function () {
    $(this).ajaxStart(
        function () {
            $.blockUI({
                message: $('#loadingMessagePanel').html(),
                css: {
                    left: '45%',
                    width: '10%',
                    padding: '10px 0'
                }
            });
        }).ajaxStop($.unblockUI);
});

//Ajax error handling
function handleError(jqXHR) {

    //Get the message
    var validationResult = $.parseJSON(jqXHR.responseText);

    //Show
    showMessagePanel(validationResult);
}

$("#messagePanelClear").live('click', function () {
    hideMessagePanel();
});

function showMessagePanel(message) {
    $('#messagePanelMessage').text(message);
    $('#messagePanel').fadeTo(200, 1);
}

function hideMessagePanel() {
    $('#messagePanel').fadeTo(200, 0.01); 
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
