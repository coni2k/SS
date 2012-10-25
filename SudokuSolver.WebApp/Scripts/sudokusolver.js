/* JSHint globals */
/*global ko:true, Enumerable:true */

"use strict";

// + Variables

// Base
var apiUrlBase = 'api/Sudoku/';

// Application level
var apiUrlSudokuList = apiUrlBase + 'sudokulist';
var apiUrlNewSudoku = apiUrlBase + 'newsudoku';
var apiUrlResetList = apiUrlBase + 'resetlist';

// Sudoku level - gets
var apiUrlSquares = apiUrlBase + 'squares/'; // + sudokuId
var apiUrlNumbers = apiUrlBase + 'numbers/'; // + sudokuId
var apiUrlHints = apiUrlBase + 'hints/'; // + sudokuId
var apiUrlAvailabilities = apiUrlBase + 'availabilities/'; // + sudokuId
//var apiUrlAvailabilities2 = apiUrlBase + 'availabilities/'; // + sudokuId
var apiUrlGroupNumberAvailabilities = apiUrlBase + 'groupnumberavailabilities/'; // + sudokuId

// Sudoku level - posts
var apiUrlUpdateSquare = apiUrlBase + 'updatesquare/'; // + sudokuId
var apiUrlToggleReady = apiUrlBase + 'toggleready/'; // + sudokuId
var apiUrlToggleAutoSolve = apiUrlBase + 'toggleautosolve/'; // + sudokuId
var apiUrlSolve = apiUrlBase + 'solve/'; // + sudokuId
var apiUrlReset = apiUrlBase + 'reset/'; // + sudokuId

var appViewModel = null;

// And start the app
$(function () {

    // New model + apply ko.bindings
    appViewModel = new AppViewModel();
    ko.applyBindings(appViewModel);

    // Load sudoku list
    appViewModel.LoadSudokuList();

});

// + Objects

function AppViewModel() {

    var self = this;
    self.SudokuList = ko.observableArray([]);
    self.Sudoku = ko.observable(new Sudoku());

    // Css
    self.HasSudokuList = ko.computed(function () { return self.SudokuList().length > 0; });
    self.HasSudoku = ko.computed(function () { return self.Sudoku().SudokuId() > 0; });

    // Load list
    self.LoadSudokuList = function () {
        getApiData(apiUrlSudokuList, function (sudokuList) {
            self.SudokuList([]);
            self.SudokuList(sudokuList);
            self.LoadSudoku(sudokuList[0]);
        });
    };

    // Select sudoku
    self.SelectSudoku = function (selectedSudokuData) { self.LoadSudoku(selectedSudokuData); };

    // Load sudoku
    self.LoadSudoku = function (sudokuData) {

        // Determines whether a new sudoku needs to be initialized or the existing one will be refreshed
        var needNewSudoku = (self.Sudoku().Size !== sudokuData.Size);

        if (needNewSudoku) {
            self.Sudoku(new Sudoku(sudokuData.Size));
        }

        // Load sudoku
        self.Sudoku().SudokuId(sudokuData.SudokuId);
        self.Sudoku().Title(sudokuData.Title);
        self.Sudoku().Description(sudokuData.Description);
        // self.Sudoku().Size(sudokuData.Size);
        self.Sudoku().SquaresLeft(sudokuData.SquaresLeft);
        self.Sudoku().Ready(sudokuData.Ready);
        self.Sudoku().AutoSolve(sudokuData.AutoSolve);

        // Grid
        //if (initOrRefreshSudoku) {
        //    self.Sudoku().InitSudoku();
        //}
        //else {
        //    self.Sudoku().RefreshGrid();
        //}

        // Load details
        self.Sudoku().LoadDetails();
    };

    // New
    self.NewSudoku = function () {

        $('#newSudokuDialog').dialog({
            resizable: false,
            height: 275,
            width: 348,
            modal: true,
            buttons: {
                'Create': function () {

                    // Validate the form
                    var isValid = $('#newSudokuForm').validate().form();
                    if (!isValid) {
                        return false;
                    }

                    // Close the dialog
                    $(this).dialog('close');

                    // Get the variables
                    var size = $('#newSudokuSize').val();
                    var title = $('#newSudokuTitle').val();
                    var description = $('#newSudokuDescription').val();

                    // Prepare the data
                    var sudokuContainer = JSON.stringify({ Size: size, Title: title, Description: description });

                    // Post
                    postApi(apiUrlNewSudoku, sudokuContainer, function (newSudoku) {

                        // Add the item to the list
                        self.SudokuList.push(newSudoku);

                        // Load the sudoku
                        self.LoadSudoku(newSudoku);

                    });
                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    };

    // Reset
    self.ResetList = function () {

        $('#resetListDialog').dialog({
            resizable: false,
            height: 200,
            width: 450,
            modal: true,
            open: function () {
                $(this).parent().find('.ui-dialog-buttonpane button:eq(1)').focus();
            },
            buttons: {
                'Reset list': function () {
                    $(this).dialog('close');

                    // Post + load
                    postApi(apiUrlResetList, null, function () { self.LoadSudokuList(); });

                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    };
}

// Sudoku
function Sudoku(size) {
    var self = this;

    // Variables
    self.SudokuId = ko.observable(0);
    self.Title = ko.observable('');
    self.Description = ko.observable('');
    self.Size = size;
    self.TotalSize = self.Size * self.Size;
    self.SquareRootofSize = Math.sqrt(self.Size);

    // Dynamic properties
    self.SquaresLeft = ko.observable(0);
    self.SelectedSquare = ko.observable(null);
    self.SelectedNumber = ko.observable(null);
    self.Ready = ko.observable(false);
    self.ReadyFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.Ready().toString());
    });
    self.AutoSolve = ko.observable(false);
    self.AutoSolveFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.AutoSolve().toString());
    });

    // Arrays
    self.Groups = new Array();
    self.NumberGroups = new Array();
    self.Hints = ko.observableArray([]);
    self.GroupNumberAvailabilities = ko.observableArray([]);

    // Css
    self.CssSize = (self.Size === 4 ? 'size4' : self.Size === 9 ? 'size9' : 'size16');

    // Square groups loop
    for (var groupCounter = 1; groupCounter <= self.Size; groupCounter++) {

        // Create group
        var group = new Group(groupCounter, self);
        // group.IsOdd = (groupCounter % 2 === 0);
        // group.CssClass = 'groupItem ' + self.CssSize;

        // Squares loop
        for (var squareCounter = 1; squareCounter <= self.Size; squareCounter++) {

            // Create square
            var squareId = squareCounter + ((groupCounter - 1) * self.Size);
            var square = new Square(squareId, group);

            // Availability loop
            for (var availabilityCounter = 0; availabilityCounter < self.Size; availabilityCounter++) {

                // Create availability item
                var availability = new Availability(square);
                availability.Value = availabilityCounter + 1;
                square.Availabilities.push(availability);

                // TODO NEW BLOCK!
                // Create availability2 item
                //var availability2 = new Availability2();
                //availability2.Value = availabilityCounter + 1;
                //square.Availabilities2.push(availability2);
            }

            group.Squares.push(square);
        }

        self.Groups.push(group);
    }

    // Numbers
    // Group the numbers, to be able to display them nicely (see numbersPanel on default.html)
    for (groupCounter = 1; groupCounter <= self.SquareRootofSize; groupCounter++) {

        var numberGroup = new SudokuNumberGroup(groupCounter, self);
        // numberGroup.CssClass = 'groupItem ' + self.CssSize;
        // numberGroup.IsOdd = (groupCounter % 2 === 0);
        // numberGroup.Numbers = new Array();

        for (var numberCounter = 1; numberCounter <= self.SquareRootofSize; numberCounter++) {

            var sudokuNumber = new SudokuNumber(numberGroup);
            sudokuNumber.Value = numberCounter + ((groupCounter - 1) * self.SquareRootofSize);
            numberGroup.Numbers.push(sudokuNumber);
        }

        self.NumberGroups.push(numberGroup);
    }

    // Grids;
    // a. Value grid
    self.ValueGrid = new ValueGrid(self.Groups);

    // b. Availability grid
    self.AvailabilityGrid = new AvailabilityGrid(self.Groups);
    self.ToggleDisplayAvailabilities = function () {
        self.AvailabilityGrid.Visible(!self.AvailabilityGrid.Visible());
    };
    self.DisplayAvailabilitiesFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.AvailabilityGrid.Visible().toString());
    });

    // c. Availability2 grid
    //self.Availability2Grid = new Availability2Grid(self.Groups);
    //self.ToggleDisplayAvailabilities = function () {
    //    self.AvailabilityGrid.Visible(!self.AvailabilityGrid.Visible());
    //}
    //self.DisplayAvailabilitiesFormatted = ko.computed(function () {
    //    return capitaliseFirstLetter(self.AvailabilityGrid.Visible().toString());
    //});

    // d. Id grid
    self.IdGrid = new IDGrid(self.Groups);
    self.ToggleDisplayIDs = function () {
        self.IdGrid.Visible(!self.IdGrid.Visible());
    };
    self.DisplayIDsFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.IdGrid.Visible().toString());
    });

    // Classes
    self.CssValueGrid = 'panel gridPanel ' + self.CssSize;
    self.CssAvailabilityGrid = ko.computed(function () { return 'panel gridPanel ' + self.CssSize + (self.AvailabilityGrid.Visible() ? '' : ' hide'); });
    self.CssIdGrid = ko.computed(function () { return 'panel gridPanel ' + self.CssSize + (self.IdGrid.Visible() ? '' : ' hide'); });

    // Methods
    self.SetSelectedSquare = function (square) {

        // Remove the previous selected square
        if (self.SelectedSquare() !== null) {
            self.SelectedSquare().SetActiveSelect(false);
        }

        // If the new value is null
        if (square === null) {
            self.SelectedSquare(null);
            return;
        }

        // If there is no selected number
        if (self.SelectedNumber() === null) {
            self.SelectedSquare(square);
            square.SetActiveSelect(true); // Then set the new square as selected
        }
        else {
            self.UpdateSelectedSquare(square, self.SelectedNumber().Value);
        }
    };

    // Selected number
    self.SetSelectedNumber = function (sudokuNumber) {

        // Remove the previous selected number
        if (self.SelectedNumber() !== null) {
            self.SelectedNumber().SetActiveSelect(false);
        }

        // If the new value is null
        if (sudokuNumber === null) {
            self.SelectedNumber(null);
            return;
        }

        // If there is no selected square
        if (self.SelectedSquare() === null) {
            self.SelectedNumber(sudokuNumber);
            sudokuNumber.SetActiveSelect(true); // Then set the new number as selected
        }
        else {
            self.UpdateSelectedSquare(self.SelectedSquare(), sudokuNumber.Value);
        }
    };

    // Update selected square
    self.UpdateSelectedSquare = function (square, newValue) {

        // Prepare the data
        var squareContainer = JSON.stringify({ SquareId: square.SquareId, Value: newValue });

        // Post + load
        postApi(apiUrlUpdateSquare + self.SudokuId(), squareContainer, function () { self.LoadDetails(); });
    };

    // Remove the value of selected square
    // TODO This can on square level?
    self.IsSelectedSquareValueRemoveable = ko.computed(function () {

        // If it's selected square is null
        if (self.SelectedSquare() === null) {
            return false;
        }

        // Or it has no value already (value is 0)
        if (self.SelectedSquare().IsAvailable()) {
            return false;
        }

        // Or the sudoku is in ready state and it has an initial value
        if (self.Ready() && self.SelectedSquare().AssignType() === 0) {
            return false;
        }

        // It's removeable
        return true;
    });

    self.RemoveSelectedSquareValue = function () {
        self.UpdateSelectedSquare(self.SelectedSquare(), 0);
    };

    // Toggle ready
    self.ToggleReady = function () {
        postApi(apiUrlToggleReady + self.SudokuId(), null, function () {

            // Update client
            self.Ready(!self.Ready());
        });
    };

    // Toggle auto solve
    self.ToggleAutoSolve = function () {
        postApi(apiUrlToggleAutoSolve + self.SudokuId(), null, function () {

            // Update client
            self.AutoSolve(!self.AutoSolve());

            // If autosolve, load details
            if (self.AutoSolve()) {
                self.LoadDetails();
            }
        });
    };

    // Solve
    self.Solve = function () {
        postApi(apiUrlSolve + self.SudokuId(), null, function () { self.LoadDetails(); });
    };

    // Reset
    self.Resettable = ko.computed(function () {
        return Enumerable.From(self.Groups).Any(function (group) {
            return Enumerable.From(group.Squares).Any(function (square) {
                if (self.Ready()) {
                    // If it's ready, check whether there are any squares that set by user, solver or hint
                    return square.AssignType() === 1 || square.AssignType() === 2 || square.AssignType() === 3;
                }
                else {
                    // If it's not ready, check whether there are any squares that have a value
                    return square.AssignType() === 0 && square.Value() !== 0;
                }
            });
        });
    });

    self.Reset = function () {

        $('#resetDialog').dialog({
            resizable: false,
            height: 200,
            width: 450,
            modal: true,
            open: function () {
                $(this).parent().find('.ui-dialog-buttonpane button:eq(1)').focus();
            },
            buttons: {
                'Reset': function () {
                    $(this).dialog('close');
                    postApi(apiUrlReset + self.SudokuId(), null, function () { self.LoadDetails(); });
                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    };

    // Load methods
    self.LoadDetails = function () {

        // Load squares
        self.LoadSquares();

        // Load numbers
        self.LoadNumbers();

        // Load hints
        self.LoadHints();

        // Load availabilities
        self.LoadAvailabilities();

        // Load availabilities
        // self.LoadAvailabilities2();

        // Load group number availabilities
        // self.LoadGroupNumberAvailabilities();

    };

    self.LoadSquares = function () {

        getApiData(apiUrlSquares + self.SudokuId(), function (squareList) {

            Enumerable.From(squareList).ForEach(function (squareItem) {

                var square = self.FindSquareBySquareId(squareItem.Id);
                square.Value(squareItem.Number.Value);
                square.AssignType(squareItem.AssignType);

            });
        });
    };

    self.LoadNumbers = function () {

        // Get the numbers from the server
        getApiData(apiUrlNumbers + self.SudokuId(), function (numberList) {

            // Zero value (SquaresLeft on detailsPanel)
            var zeroNumber = numberList.splice(0, 1);
            self.SquaresLeft(zeroNumber[0].Count);

            // Count of other numbers
            Enumerable.From(numberList).ForEach(function (numberItem) {

                Enumerable.From(self.NumberGroups).ForEach(function (group) {

                    Enumerable.From(group.Numbers).ForEach(function (number) {

                        if (number.Value === numberItem.Value) {
                            number.Count(numberItem.Count);
                            return;
                        }

                    });
                });
            });
        });
    };

    self.LoadHints = function () {

        // Get the hints from the server
        getApiData(apiUrlHints + self.SudokuId(), function (hintList) {

            // Reset the current list
            self.Hints([]);

            // Add the new hints to the list
            Enumerable.From(hintList).ForEach(function (hintItem) {

                // Create hint
                var hint = new Hint(self);
                hint.SquareId = hintItem.Square.Id;
                hint.HintValue = hintItem.Number.Value;
                hint.HintType = hintItem.Type;

                self.Hints.push(hint);
            });
        });
    };

    self.LoadAvailabilities = function () {

        // Get the data
        getApiData(apiUrlAvailabilities + self.SudokuId(), function (availabilityList) {

            Enumerable.From(availabilityList).ForEach(function (availabilityItem) {

                // Get the square
                var square = self.FindSquareBySquareId(availabilityItem.SquareId);

                // Get the availability
                var availability = Enumerable.From(square.Availabilities).Single(function (availability) {
                    return availability.Value === availabilityItem.Number.Value;
                });

                // Set IsAvailable
                availability.IsAvailable(availabilityItem.IsAvailable);

            });
        });
    };

    //self.LoadAvailabilities2 = function() {

    //    getApiData(apiUrlAvailabilities2 + self.SudokuId(), function (list) {

    //        $.each(list, function () {

    //            //Number
    //            var number = this.Number;

    //            //Find the square
    //            var matchedSquare = self.FindSquareBySquareId(this.SquareId);

    //            //Find the availability
    //            var matched = ko.utils.arrayFirst(matchedSquare.Availabilities2(), function (availability2) {
    //                return availability2.Value === number;
    //            });

    //            //Update
    //            matched.IsAvailable(this.IsAvailable);

    //        });
    //    });
    //}

    self.LoadGroupNumberAvailabilities = function () {

        getApiData(apiUrlGroupNumberAvailabilities + self.SudokuId(), function (list) {

            self.GroupNumberAvailabilities([]);

            Enumerable.From(list).ForEach(function (groupNumberAvailabilityItem) {

                var groupNumberAvailability = new GroupNumberAvailability();
                groupNumberAvailability.GroupId = groupNumberAvailabilityItem.GroupId;
                groupNumberAvailability.Number = groupNumberAvailabilityItem.Number;
                groupNumberAvailability.Count = groupNumberAvailabilityItem.Count;
                self.GroupNumberAvailabilities.push(groupNumberAvailability);

            });
        });
    };

    // Filters
    // a. Find square by squareId
    self.FindSquareBySquareId = function (squareId) {

        var matchedSquare = null;

        // Loop through groups
        Enumerable.From(self.Groups).ForEach(function (group) {

            // Search for the square with squareId
            matchedSquare = Enumerable.From(group.Squares).SingleOrDefault(null, function (square) {
                return square.SquareId === squareId;
            });

            // If there is, break the loop
            if (matchedSquare !== null) {
                return false;
            }
        });

        return matchedSquare;
    };

    // b. Find square by number
    self.FindSquareByNumber = function (number) {

        var matchedSquares = new Array([]);

        // Loop through groups
        Enumerable.From(self.Groups).ForEach(function (group) {

            // Search for the squares with the number
            var matchedSquare = Enumerable.From(group.Squares).SingleOrDefault(null, function (square) {
                return square.Value() === number;
            });

            // If there is, add it to the list
            if (matchedSquare !== null) {
                matchedSquares.push(matchedSquare);
            }
        });

        return matchedSquares;
    };
}

function ValueGrid(groups) {
    var self = this;
    self.Groups = groups;
    self.DisplayMode = 'value';
    self.Template = 'squareValueTemplate';
    // Visible = always!
}

function AvailabilityGrid(groups) {
    var self = this;
    self.Groups = groups;
    self.DisplayMode = 'availability';
    self.Template = 'squareAvailabilitiesTemplate';
    self.Visible = ko.observable(true);
}

//function Availability2Grid(groups) {
//    var self = this;
//    self.Groups = ko.observableArray(groups);
//    self.DisplayMode = 'availability2';
//    //self.Visible = ko.observable(true);
//    self.Visible = true; //ko.observable(true);
//}

function IDGrid(groups) {
    var self = this;
    self.Groups = groups;
    self.DisplayMode = 'id';
    self.Template = 'squareIdTemplate';
    self.Visible = ko.observable(false);
}

function Group(groupId, sudoku) {
    var self = this;
    self.GroupId = groupId;
    self.Sudoku = sudoku;
    self.Squares = new Array();
    self.IsOdd = (self.GroupId % 2 === 0);
    self.CssClass = 'groupItem ' + self.Sudoku.CssSize;
}

function Square(squareId, group) {
    var self = this;
    self.SquareId = squareId;
    self.Group = group;
    self.Value = ko.observable(0);
    self.AssignType = ko.observable(0);

    self.ValueFormatted = ko.computed(function () {
        return self.Value() === 0 ? '&nbsp;' : self.Value();
    });

    self.IsAvailable = ko.computed(function () { return self.Value() === 0; });

    // self.IsUpdateable = ko.computed(function () { return !(self.Group.Sudoku.Ready() && self.AssignType() === 0 && !self.IsAvailable()); });

    self.Availabilities = new Array();
    //self.Availabilities2 = ko.observableArray([]);

    // Passive select
    self.IsPassiveSelected = ko.observable(false);
    self.TogglePassiveSelect = function (data, event) {
        self.IsPassiveSelected(event.type === 'mouseenter');
    };

    // Active select
    self.IsActiveSelected = ko.observable(false);
    self.SetActiveSelect = function (isActive) {

        // Set IsActiveSelected property
        self.IsActiveSelected(isActive);

        // Related square selected
        Enumerable.From(self.Group.Squares).Where(function (square) {
            return square !== self;
        }).ForEach(function (square) {
            square.IsRelatedSelected(isActive);
        });
    };

    // Related selected
    self.IsRelatedSelected = ko.observable(false);

    // Css
    //self.CssAssignType = ko.computed(function () {
    //    switch (self.AssignType()) {
    //        case 0:
    //            return 'initial';
    //        case 1:
    //            return 'user';
    //        case 2:
    //            return 'hint';
    //        case 3:
    //            return 'solver';
    //    }
    //});

    //self.CssClassOld = ko.computed(function () {
    //    return 'squareItem ' + self.CssAssignType() + ' '
    //        + self.Group.Sudoku.CssSize
    //        + (self.IsPassiveSelected() ? ' passiveSelected' : '')
    //        + (self.IsActiveSelected() ? ' activeSelected' : '')
    //        + (self.IsRelatedSelected() ? ' relatedSelected' : '')
    //        + (self.Group.IsOdd ? ' odd' : '');
    //});

    self.CssClass = 'squareItem ' + self.Group.Sudoku.CssSize;

}

function SudokuNumberGroup(numberGroupId, sudoku) {
    var self = this;
    self.NumberGroupId = numberGroupId;
    self.Sudoku = sudoku;
    self.Numbers = new Array();
    self.IsOdd = (self.NumberGroupId % 2 === 0);
    self.CssClass = 'groupItem ' + self.Sudoku.CssSize;
}

function SudokuNumber(group) {
    var self = this;
    self.Group = group;
    self.Value = 0;
    self.Count = ko.observable(0);

    // Passive select (for mouseenter + leave)
    self.IsPassiveSelected = ko.observable(false);
    self.TogglePassiveSelect = function (data, event) {
        self.IsPassiveSelected(event.type === 'mouseenter');
    };

    // Active select
    self.IsActiveSelected = ko.observable(false);
    self.SetActiveSelect = function (isActive) {

        // Set IsActiveSelected property
        self.IsActiveSelected(isActive);

        // Related square selected
        Enumerable.From(self.Group.Sudoku.FindSquareByNumber(self.Value)).ForEach(function (square) {
            square.IsRelatedSelected(isActive);
        });
    };

    // Css
    //self.CssClass = ko.computed(function () {
    //    // css: { size4: $parents[1].Size() === 4, size9: $parents[1].Size() === 9, size16: $parents[1].Size() === 16, passiveSelected: IsPassiveSelected(), activeSelected: IsActiveSelected(), odd: $parent.IsOdd
    //    return 'squareItem ' + self.Group.Sudoku.CssSize + (self.IsPassiveSelected() ? ' passiveSelected' : '') + (self.IsActiveSelected() ? ' activeSelected' : '') + (self.Group.IsOdd ? ' odd' : '');
    //});

    self.CssClass = 'squareItem ' + self.Group.Sudoku.CssSize;

}

function Hint(sudoku) {
    var self = this;
    self.Sudoku = sudoku;
    self.SquareId = 0;
    self.HintValue = 0;
    self.HintType = 0;
    self.IsSelected = ko.observable(false);
    self.ToggleSelect = function (data, event) {

        // Toggle select itself
        self.IsSelected(event.type === 'mouseenter');

        // Find & toggle select related square as well
        var relatedSquare = data.Sudoku.FindSquareBySquareId(data.SquareId);
        relatedSquare.Value(event.type === 'mouseenter' ? self.HintValue : 0);
        relatedSquare.AssignType(event.type === 'mouseenter' ? 2 : 0);
        relatedSquare.TogglePassiveSelect(relatedSquare, event);
    };
}

function Availability(square) {
    var self = this;
    self.Square = square;
    self.Value = 0;
    self.IsAvailable = ko.observable(true);

    // css: { unavailable_self: !$parent.IsAvailable(), unavailable_group: !IsAvailable() }
    // self.CssClass = ko.computed(function () {

        //var cssClass = 'availabilityItem';

        //if (!self.Square.IsAvailable())
        //    cssClass += ' unavailable_self';

        //if (!self.IsAvailable())
        //    cssClass += ' unavailable_group';

        // return 'availabilityItem' + (!self.Square.IsAvailable() ? ' unavailable_self' : '') + (!self.IsAvailable() ? ' unavailable_group' : '');
    // });
}

//function Availability2() {
//    var self = this;
//    self.Value = 0;
//    self.IsAvailable = ko.observable(true);
//}

function GroupNumberAvailability() {
    var self = this;
    self.GroupId = 0;
    self.Number = 0;
    self.Count = 0;
}

// + Global methods + setup

// jQuery ajax
$.ajaxSetup({
    contentType: 'application/json; charset=utf-8'
});

// Get data from server
function getApiData(apiUrl, callback) {

    $.getJSON(apiUrl, function (data) {

        if (callback !== null) {
            callback(data);
        }

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

// Post to server
function postApi(apiUrl, postData, callback) {

    $.post(apiUrl, postData, function (data) {

        if (callback !== null) {
            callback(data);
        }

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

// Loading message during ajax
$(function () {
    $(this).ajaxStart(function () {

        // Hide previous error message
        hideMessagePanel();

        $('#loadingMessagePanel').dialog('open');

    }).ajaxStop(function () {
        $('#loadingMessagePanel').dialog('close');
    });
});

// Error handling
function handleError(jqXHR) {

    // Get the message
    var validationResult = $.parseJSON(jqXHR.responseText).Message;

    // Show
    showMessagePanel(validationResult);
}

// Handle the delete key; remove selected square's value
$(document).keydown(function (e) {

    if (e.keyCode !== 46) {
        return;
    }

    if (appViewModel.Sudoku().SelectedSquare() === null) {
        return;
    }

    appViewModel.Sudoku().RemoveSelectedSquareValue();
});

// Handle the escape key; remove selected square
$(document).keyup(function (e) {

    if (e.keyCode !== 27) {
        return;
    }

    if (appViewModel.Sudoku().SelectedSquare() !== null) {
        appViewModel.Sudoku().SetSelectedSquare(null);
    }

    if (appViewModel.Sudoku().SelectedNumber() !== null) {
        appViewModel.Sudoku().SetSelectedNumber(null);
    }
});

function capitaliseFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

// Panels
$("#loadingMessagePanel").dialog({
    dialogClass: 'ui-dialog-notitlebar',
    resizable: false,
    height: 70,
    width: 250,
    modal: true
});

$('#messagePanelClear').live('click', function () {
    hideMessagePanel();
});

function showMessagePanel(message) {
    $('#messagePanelMessage').text(message);
    $('#messagePanel').fadeTo(200, 1);
}

function hideMessagePanel() {
    $('#messagePanel').fadeTo(200, 0.01);
}
