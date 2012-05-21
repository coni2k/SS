
function SudokuViewModel() {
    var self = this;

    //Sudoku list
    self.SudokuList = ko.observableArray([]);
    self.LoadSudoku = function (sudoku) { loadSudoku(sudoku); }
    self.NewSudoku = function () { newSudoku(self); }
    self.ResetList = function () { resetList(); }

    //Id, Size, SquaresLeft
    self.SudokuId = ko.observable(0);
    self.Size = ko.observable(0);
    self.TotalSize = ko.computed(function () { return self.Size() * self.Size(); });
    self.SquareRootofSize = ko.computed(function () { return Math.sqrt(self.Size()); });
    self.SquaresLeft = ko.observable(0);

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

function IDGrid(groups) {
    var self = this;
    self.Groups = ko.observableArray(groups);
    self.DisplayMode = 'id';
    self.Visible = ko.observable(false);
}

function Group() {
    var self = this;
    self.GroupId = ko.observable(0);
    self.Squares = ko.observableArray([]);
    self.IsOdd = ko.computed(function () { return self.GroupId() % 2 === 0; });
}

function Square(group, id) {
    var self = this;
    self.Group = group;
    self.SquareId = id;
    self.Number = ko.observable(''); //Value instead of Number?
    //self.NumberFormatted = ko.computed(function () { return self.Number() === 0 ? '' : self.Number().toString() });
    self.AssignType = ko.observable(0);
    self.Availabilities = ko.observableArray([]);
    self.IsSelected = ko.observable(false);
    self.ToggleSelect = function (data, event) {
        self.IsSelected(event.type == 'mouseenter');
    }

    self.Updating = function (data, event) {

        // Ensure that it is a valid number

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

            //Reset first?
            data.Number('');

            //Ensure that the value is smaller than sudoku size
            var newValue = $(event.target).val() + String.fromCharCode(event.keyCode);
            if (newValue > sudokuViewModel.Size()) {
                event.preventDefault();
            }
        }

        return true;
    }

    self.Update = function (data, event) {

        var number = data.Number();
        if (number === '')
            number = 0;

        //Prepare post data
        var squareContainer = { SquareId: data.SquareId, Number: number };
        var json = JSON.stringify(squareContainer);

        $.ajax({
            url: 'api/SudokuApi/updatesquare/' + sudokuViewModel.SudokuId(),
            type: 'POST',
            data: json,
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //If it's "AutoSolve", load it from the server
                    if (sudokuViewModel.AutoSolve()) {
                        loadUsedSquares(sudokuViewModel.SudokuId());
                    }

                    //Load numbers
                    loadNumbers(sudokuViewModel.SudokuId());

                    //Load potentials
                    loadPotentials(sudokuViewModel.SudokuId());

                    //Load availabilities
                    loadAvailabilities(sudokuViewModel.SudokuId());

                },
                400 /* BadRequest */: function (jqxhr) {
                    var validationResult = $.parseJSON(jqxhr.responseText);

                    //Show message
                    showMessagePanel(validationResult);

                    //Clear the square
                    data.Number('');
                }
            }
        });
    }
}

function SudokuNumber() {
    var self = this;
    self.Value = 0;
    self.Count = ko.observable(0);
    self.IsSelected = ko.observable(false);
    self.ToggleSelect = function (data, event) {

        //Toggle select itself
        self.IsSelected(event.type == 'mouseenter');

        //Find & toggle select related square as well
        var relatedSquares = findSquaresByNumber(data.Value);
        $.each(relatedSquares, function () {
            this.ToggleSelect(null, event);
        });
    }
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
    }
}

function Availability() {
    var self = this;
    self.Value = 0; //Number instead of Value?
    self.IsAvailable = ko.observable(true);
}

function loadSudokuList() {

    $.get('api/SudokuApi/list', function (sudokuList) {

        sudokuViewModel.SudokuList([]);
        sudokuViewModel.SudokuList(sudokuList);

        loadSudoku(sudokuViewModel.SudokuList()[0]);
    });
}

function loadSudoku(sudoku) {

    //Determines whether the a new grid needs to be initialized or just the existing one will be refreshed
    var initOrRefreshGrid = sudokuViewModel.Size() !== sudoku.Size;

    //Load sudoku
    sudokuViewModel.SudokuId(sudoku.SudokuId);
    sudokuViewModel.Size(sudoku.Size);
    sudokuViewModel.Ready(sudoku.Ready);
    sudokuViewModel.AutoSolve(sudoku.AutoSolve);

    //Grid
    if (initOrRefreshGrid)
        initGrid(sudoku.Size);
    else
        refreshGrid();

    //Load used squares
    loadUsedSquares(sudoku.SudokuId);

    //Load numbers
    loadNumbers(sudoku.SudokuId);

    //Load potentials
    loadPotentials(sudoku.SudokuId);

    //Load availabilities
    loadAvailabilities(sudoku.SudokuId);

}

function initGrid(size) {

    //Create an array for square type groups
    sudokuViewModel.Groups([]);
    for (groupCounter = 1; groupCounter <= size; groupCounter++) {

        //Create group
        var group = new Group();
        group.GroupId(groupCounter);

        //Squares loop
        for (var squareCounter = 1; squareCounter <= size; squareCounter++) {

            //Create square
            var squareId = calculateSquareId(size, groupCounter, squareCounter);
            square = new Square(group, squareId);
            //square.SquareId = calculateSquareId(size, groupCounter, squareCounter);
            //square.SquareId = squareCounter + ((groupCounter - 1) * size);

            //TODO Is it correct way of doing it?
            if (sudokuViewModel.Ready())
                square.AssignType(1);

            //Availability loop
            for (var availabilityCounter = 0; availabilityCounter < size; availabilityCounter++) {

                //Create availability item
                var availability = new Availability();
                availability.Value = availabilityCounter + 1;
                square.Availabilities.push(availability);
            }

            group.Squares.push(square);
        }

        sudokuViewModel.Groups.push(group);
    }
}

function refreshGrid() {

    ko.utils.arrayForEach(sudokuViewModel.Groups(), function (group) {
        ko.utils.arrayForEach(group.Squares(), function (square) {
            square.Number('');
            square.AssignType(sudokuViewModel.Ready() ? 1 : 0);
            ko.utils.arrayForEach(square.Availabilities(), function (availability) {
                availability.IsAvailable(true);
            });
        });
    });
}

//Is it possible to retrieve only the changes?
function loadUsedSquares(sudokuId) {

    $.get('api/SudokuApi/usedsquares/' + sudokuId, function (usedSquareList) {

        $.each(usedSquareList, function () {

            //Find the square
            var matchedSquare = findSquareBySquareId(this.SquareId);

            //Update
            matchedSquare.Number(this.Number === 0 ? '' : this.Number);
            matchedSquare.AssignType(this.AssignType);
        });
    });
}

function loadNumbers(sudokuId) {

    $.get('api/SudokuApi/numbers/' + sudokuId, function (numberList) {

        //Zero value (SquaresLeft on detailsPanel)
        var zeroNumber = numberList.splice(0, 1);
        sudokuViewModel.SquaresLeft(zeroNumber[0].Count);

        //TODO Why cant it be used without assigning to a local?
        var sqrtSize = sudokuViewModel.SquareRootofSize();

        //Group the numbers, to be able to display them nicely (see numbersPanel on default.html)
        sudokuViewModel.NumberGroups([]);
        for (groupCounter = 0; groupCounter < sqrtSize; groupCounter++) {

            var numberGroup = new Object();
            numberGroup.Numbers = new Array();

            for (numberCounter = 0; numberCounter < sqrtSize; numberCounter++) {

                var numberData = numberList.splice(0, 1);

                var sudokuNumber = new SudokuNumber();
                sudokuNumber.Value = numberData[0].Value;
                sudokuNumber.Count(numberData[0].Count);
                numberGroup.Numbers.push(sudokuNumber);
            }

            sudokuViewModel.NumberGroups.push(numberGroup);
        };
    });
}

function loadPotentials(sudokuId) {

    $.get('api/SudokuApi/potentials/' + sudokuId, function (potentialList) {

        sudokuViewModel.Potentials([]);

        $.each(potentialList, function () {

            //Create potential
            var potential = new Potential();
            potential.SquareId = this.SquareId;
            potential.PotentialValue = this.PotentialValue;
            potential.PotentialType = this.PotentialType;
            sudokuViewModel.Potentials.push(potential);
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

function newSudoku(model) {

    $.ajax({
        url: 'api/SudokuApi/newsudoku',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        statusCode: {
            201 /*Created*/: function (newSudoku) {

                //Add the item to the list
                model.SudokuList.push(newSudoku);

                //Load the sudoku
                //TODO What to load, just init or refresh the grid?
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

function toggleReady(model) {

    $.ajax({
        url: 'api/SudokuApi/toggleready/' + model.SudokuId(),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        statusCode: {
            200 /*OK*/: function () {

                //Toggle
                self.Ready(!model.Ready());

                //Assign Types
                ko.utils.arrayForEach(model.Groups(), function (group) {
                    ko.utils.arrayForEach(group.Squares(), function (square) {
                        if (square.Number() === '')
                            square.AssignType(model.Ready() ? 1 : 0);
                    });
                });
            },
            400 /* BadRequest */: function (jqxhr) {
                var validationResult = $.parseJSON(jqxhr.responseText);

                //Show message
                showMessagePanel(validationResult);
            }
        }
    });
}

function toggleAutoSolve(model) {

    $.ajax({
        url: 'api/SudokuApi/toggleautosolve/' + model.SudokuId(),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        statusCode: {
            200 /*OK*/: function () {

                //Update UI
                model.AutoSolve(!model.AutoSolve());

                if (model.AutoSolve()) {

                    //Load used squares
                    loadUsedSquares(model.SudokuId());

                    //Load numbers
                    loadNumbers(model.SudokuId());

                    //Load potentials
                    loadPotentials(model.SudokuId());
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

function solve(model) {

    $.ajax({
        url: 'api/SudokuApi/solve/' + model.SudokuId(),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        statusCode: {
            200 /*OK*/: function () {

                //Load used squares
                loadUsedSquares(model.SudokuId());

                //Load numbers
                loadNumbers(model.SudokuId());

                //Load potentials
                loadPotentials(model.SudokuId());

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
    ko.utils.arrayFirst(sudokuViewModel.Groups(), function (group) {

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
//And also is it bit slow?
function findSquaresByNumber(number) {

    var matchedSquares = new Array();

    //Loop through groups
    ko.utils.arrayForEach(sudokuViewModel.Groups(), function (group) {

        //Loop through squares
        ko.utils.arrayForEach(group.Squares(), function (square) {

            //If it matches, add to the list
            if (square.Number() === number)
                matchedSquares.push(square);
        });
    });

    return matchedSquares;
}

//TODO Move to a better place?
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

$("#messagePanelClear").live('click', function () {
    hideMessagePanel();
});

function showMessagePanel(message) {
    //$('#messagePanelMessage').html($('#messagePanelMessage').html() + '<br />' + message);
    $('#messagePanelMessage').html(message);
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
