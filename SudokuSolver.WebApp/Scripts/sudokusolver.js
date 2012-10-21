
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
    self.Sudoku = ko.observable(new Sudoku());
    self.SudokuList = ko.observableArray([]);

    // Load list
    self.LoadSudokuList = function () {
        getApiData(apiUrlSudokuList, function (sudokuList) {
            self.SudokuList([]);
            self.SudokuList(sudokuList);
            self.LoadSudoku(sudokuList[0]);
        });
    };

    // Select sudoku
    self.SelectSudoku = function (selectedSudokuData) { self.LoadSudoku(selectedSudokuData); }

    // Load sudoku
    self.LoadSudoku = function (sudokuData) {

        // Determines whether the a new grid needs to be initialized or the existing one will be refreshed
        var initOrRefreshSudoku = (self.Sudoku().Size() !== sudokuData.Size);

        // Load sudoku
        self.Sudoku().SudokuId(sudokuData.SudokuId);
        self.Sudoku().Title(sudokuData.Title);
        self.Sudoku().Description(sudokuData.Description);
        self.Sudoku().Size(sudokuData.Size);
        self.Sudoku().SquaresLeft(sudokuData.SquaresLeft);
        self.Sudoku().Ready(sudokuData.Ready);
        self.Sudoku().AutoSolve(sudokuData.AutoSolve);

        // Grid
        if (initOrRefreshSudoku) {
            self.Sudoku().InitSudoku();
        }
        else {
            self.Sudoku().RefreshGrid();
        }

        // Load details
        self.Sudoku().LoadDetails();
    }

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
    }

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
    }
}

// Sudoku
function Sudoku() {
    var self = this;

    // Variables
    self.SudokuId = ko.observable(0);
    self.Title = ko.observable('');
    self.Description = ko.observable('');
    self.Size = ko.observable(0);
    self.TotalSize = ko.computed(function () { return self.Size() * self.Size(); });
    self.SquareRootofSize = ko.computed(function () { return Math.sqrt(self.Size()); });
    self.SquaresLeft = ko.observable(0);
    self.Groups = ko.observableArray([]);
    self.NumberGroups = ko.observableArray([]);
    self.Hints = ko.observableArray([]);
    self.GroupNumberAvailabilities = ko.observableArray([]);
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

    // Grids;
    // a. Value grid
    self.ValueGrid = new ValueGrid(self.Groups);

    // b. Availability grid
    self.AvailabilityGrid = new AvailabilityGrid(self.Groups);
    self.ToggleDisplayAvailabilities = function () {
        self.AvailabilityGrid.Visible(!self.AvailabilityGrid.Visible());
    }
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
    }
    self.DisplayIDsFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.IdGrid.Visible().toString());
    });

    // Classes
    self.CssSize = ko.observable('');
    self.CssValueGrid = ko.computed(function () { return 'panel gridPanel ' + self.CssSize(); });
    self.CssAvailabilityGrid = ko.computed(function () { return 'panel gridPanel ' + self.CssSize() + (self.AvailabilityGrid.Visible() ? '' : ' hide'); });
    self.CssIdGrid = ko.computed(function () { return 'panel gridPanel ' + self.CssSize() + (self.IdGrid.Visible() ? '' : ' hide'); });

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
        if (self.SelectedSquare() === null)
            return false;

        // Or it has no value already (value is 0)
        if (self.SelectedSquare().IsAvailable())
            return false;

        // Or the sudoku is in ready state and it has an initial value
        if (self.Ready() && self.SelectedSquare().AssignType() === 0)
            return false;

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
    }

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
    }

    // Solve
    self.Solve = function () {
        postApi(apiUrlSolve + self.SudokuId(), null, function () { self.LoadDetails(); });
    };

    // Reset
    self.Resettable = ko.computed(function () {

        if (self.Ready()) {

            // If it's ready, check whether there are any squares that set by user, solver or hint
            return ko.utils.arrayFirst(self.Groups(), function (group) {
                return ko.utils.arrayFirst(group.Squares, function (square) {
                    return square.AssignType() === 1 || square.AssignType() === 2 || square.AssignType() === 3;
                });
            }) !== null;
        }
        else {

            // If it's not ready, check whether there are any squares that have a value
            return ko.utils.arrayFirst(self.Groups(), function (group) {
                return ko.utils.arrayFirst(group.Squares, function (square) {
                    return square.AssignType() === 0 && square.Value() !== 0;
                });
            }) !== null;
        }
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
                    postApi(apiUrlReset + self.SudokuId(), null, function () { self.LoadDetails() });
                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    };

    // Init + Refresh
    self.InitSudoku = function () {

        // Css
        self.CssSize(self.Size() === 4 ? 'size4' : self.Size() === 9 ? 'size9' : 'size16');
        //self.CssValueGrid('panel gridPanel ' + self.CssSize);
        //self.CssAvailabilityGrid('panel gridPanel ' + self.CssSize);
        //self.CssValueGrid('panel gridPanel ' + self.CssSize);

        // Squares
        // Create an array for square type groups
        self.Groups([]);
        var size = self.Size();
        for (var groupCounter = 1; groupCounter <= size; groupCounter++) {

            // Create group
            var group = new Group(self);
            group.IsOdd = (groupCounter % 2 === 0);
            group.CssClass = 'groupItem ' + self.CssSize();
            group.Squares = new Array();

            // Squares loop
            for (var squareCounter = 1; squareCounter <= size; squareCounter++) {

                // Create square
                var squareId = squareCounter + ((groupCounter - 1) * size);
                var square = new Square(group, squareId);

                // Availability loop
                for (var availabilityCounter = 0; availabilityCounter < size; availabilityCounter++) {

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
        self.NumberGroups([]);
        var sqrtSize = self.SquareRootofSize();
        for (groupCounter = 1; groupCounter <= sqrtSize; groupCounter++) {

            var numberGroup = new SudokuNumberGroup(self);
            numberGroup.CssClass = 'groupItem ' + self.CssSize();
            numberGroup.IsOdd = (groupCounter % 2 === 0);
            numberGroup.Numbers = new Array();

            for (var numberCounter = 1; numberCounter <= sqrtSize; numberCounter++) {

                var sudokuNumber = new SudokuNumber(numberGroup);
                sudokuNumber.Value = numberCounter + ((groupCounter - 1) * sqrtSize);
                numberGroup.Numbers.push(sudokuNumber);
            }

            self.NumberGroups.push(numberGroup);
        };
    }

    self.RefreshGrid = function () {

        ko.utils.arrayForEach(self.Groups(), function (group) {
            ko.utils.arrayForEach(group.Squares, function (square) {
                square.Value(0);
                square.AssignType(0);
                ko.utils.arrayForEach(square.Availabilities(), function (availability) {
                    availability.IsAvailable(true);
                });
            });
        });
    }

    // Load methods
    self.LoadDetails = function() {

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

    }

    self.LoadSquares = function() {

        getApiData(apiUrlSquares + self.SudokuId(), function (squareList) {

            $.each(squareList, function () {

                // Find the square
                var matchedSquare = self.FindSquareBySquareId(this.Id);

                // Update
                matchedSquare.Value(this.Number.Value);
                matchedSquare.AssignType(this.AssignType);
            });
        });
    }

    self.LoadNumbers = function() {

        getApiData(apiUrlNumbers + self.SudokuId(), function (numberList) {

            // Zero value (SquaresLeft on detailsPanel)
            var zeroNumber = numberList.splice(0, 1);
            self.SquaresLeft(zeroNumber[0].Count);

            $.each(numberList, function () {

                // Find the square
                var matchedNumber = self.FindNumberByNumberValue(this.Value);

                // Update
                matchedNumber.Count(this.Count);
            });
        });
    }

    self.LoadHints = function() {

        getApiData(apiUrlHints + self.SudokuId(), function (hintList) {

            // Reset
            self.Hints([]);

            $.each(hintList, function () {

                // Create hint
                var hint = new Hint(self);

                hint.SquareId = this.Square.Id;
                hint.HintValue = this.Number.Value;
                hint.HintType = this.Type;

                self.Hints.push(hint);
            });
        });
    }

    self.LoadAvailabilities = function() {

        getApiData(apiUrlAvailabilities + self.SudokuId(), function (availabilityList) {

            $.each(availabilityList, function () {

                var availabilityItem = this;

                // Number
                var number = availabilityItem.Number.Value;

                // Find the square
                var matchedSquare = self.FindSquareBySquareId(availabilityItem.SquareId);

                // Find the availability
                var matchedAvailability = ko.utils.arrayFirst(matchedSquare.Availabilities(), function (availability) {
                    return availability.Value === number;
                });

                // Update
                matchedAvailability.IsAvailable(availabilityItem.IsAvailable);

            });
        });
    }

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

    self.LoadGroupNumberAvailabilities = function() {

        getApi(apiUrlGroupNumberAvailabilities + self.SudokuId(), function (list) {

            self.GroupNumberAvailabilities([]);

            $.each(list, function () {
                var groupNumberAvailability = new GroupNumberAvailability();
                groupNumberAvailability.GroupId = this.GroupId;
                groupNumberAvailability.Number = this.Number;
                groupNumberAvailability.Count = this.Count;
                self.GroupNumberAvailabilities.push(groupNumberAvailability);
            });
        });
    }

    // Filters
    // a. Find square by squareId
    self.FindSquareBySquareId = function (squareId) {

        var matchedSquare = null;

        // Loop through groups
        ko.utils.arrayFirst(self.Groups(), function (group) {

            // Loop through squares
            matchedSquare = ko.utils.arrayFirst(group.Squares, function (square) {

                // Return if you find the square
                return square.SquareId === squareId;
            });

            // Stop groups loop as well
            return matchedSquare !== null;
        });

        return matchedSquare;

    };

    // b. Find square by number
    self.FindSquareByNumber = function (number) {

        var matchedSquares = new Array();

        // Loop through groups
        ko.utils.arrayForEach(self.Groups(), function (group) {

            // Loop through squares
            ko.utils.arrayForEach(group.Squares, function (square) {

                // If it matches, add to the list
                if (square.Value() === number) {
                    matchedSquares.push(square);
                }
            });
        });

        return matchedSquares;
    };

    // c. Find number by number value
    self.FindNumberByNumberValue = function (numberValue) {

        var matchedNumber = null;

        // Loop through groups
        ko.utils.arrayFirst(self.NumberGroups(), function (group) {

            // Loop through squares
            matchedNumber = ko.utils.arrayFirst(group.Numbers, function (sudokuNumber) {

                // Return if you find the square
                return sudokuNumber.Value === numberValue;
            });

            // Stop groups loop as well
            return matchedNumber !== null;
        });

        return matchedNumber;
    }
}

function ValueGrid(groups) {
    var self = this;
    self.Groups = ko.observableArray(groups);
    self.DisplayMode = 'value';
    self.SquareTemplateName = 'value-template';
    // Visible = always!
}

function AvailabilityGrid(groups) {
    var self = this;
    self.Groups = ko.observableArray(groups);
    self.DisplayMode = 'availability';
    self.SquareTemplateName = 'availability-template';
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
    self.Groups = ko.observableArray(groups);
    self.DisplayMode = 'id';
    self.SquareTemplateName = 'id-template';
    self.Visible = ko.observable(false);
}

function Group(sudoku) {
    var self = this;
    self.Sudoku = sudoku;
    self.Squares = null;
    self.IsOdd = false;
    self.CssClass = '';
}

function Square(group, id) {
    var self = this;
    self.Group = group;
    self.SquareId = id;
    self.Value = ko.observable(0);
    self.AssignType = ko.observable(0);

    //self.CssBase = 'squareItem';
    //self.IdCssClassId = '';

    self.ValueFormatted = ko.computed({
        read: function () {
            return self.Value() === 0 ? '' : self.Value();
        },
        write: function (value) {
            self.Value(value === '' ? 0 : value);
        }
    });

    self.IsAvailable = ko.computed(function () { return self.Value() === 0; });

    // self.IsUpdateable = ko.computed(function () { return !(self.Group.Sudoku.Ready() && self.AssignType() === 0 && !self.IsAvailable()); });

    self.Availabilities = ko.observableArray([]);
    //self.Availabilities2 = ko.observableArray([]);

    // Passive select
    self.IsPassiveSelected = ko.observable(false);
    self.TogglePassiveSelect = function (data, event) {
        self.IsPassiveSelected(event.type == 'mouseenter');
    }

    // Active select
    self.IsActiveSelected = ko.observable(false);
    self.SetActiveSelect = function (isActive) {

        // Set IsActiveSelected property
        self.IsActiveSelected(isActive);

        // Related square selected
        ko.utils.arrayForEach(self.Group.Squares, function (square) {
            if (square !== self) {
                square.IsRelatedSelected(isActive);
            }
        });
    };

    // Related selected
    self.IsRelatedSelected = ko.observable(false);

    //  css: { activeSelected: IsActiveSelected, relatedSelected: IsRelatedSelected(), passiveSelected: IsPassiveSelected(), size16: $parents[2].Size() === 16, odd: $parent.IsOdd }"
    // class="squareItem">

    self.CssAssignType = ko.computed(function () {
        switch (self.AssignType())
        {
            case 0:
                return 'initial';
            case 1:
                return 'user';
            case 2:
                return 'hint';
            case 3:
                return 'solver';
        } 
    });

    self.CssClass = ko.computed(function () {
        return 'squareItem ' + self.CssAssignType() + ' '
            + self.Group.Sudoku.CssSize()
            + (self.IsPassiveSelected() ? ' passiveSelected' : '')
            + (self.IsActiveSelected() ? ' activeSelected' : '')
            + (self.IsRelatedSelected() ? ' relatedSelected' : '')
            + (self.Group.IsOdd ? ' odd' : '');
    });
}

function SudokuNumberGroup(sudoku) {
    var self = this;
    self.Sudoku = sudoku;
    self.Numbers = null;
    self.IsOdd = false;
    self.CssClass = '';
}

function SudokuNumber(group) {
    var self = this;
    self.Group = group;
    self.Value = 0;
    self.Count = ko.observable(0);

    // Passive select (for mouseenter + leave)
    self.IsPassiveSelected = ko.observable(false);
    self.TogglePassiveSelect = function (data, event) {
        self.IsPassiveSelected(event.type == 'mouseenter');
    }

    // Active select
    self.IsActiveSelected = ko.observable(false);
    self.SetActiveSelect = function (isActive) {

    // Set IsActiveSelected property
        self.IsActiveSelected(isActive);

        // Related square selected
        //ko.utils.arrayForEach(self.Group.Squares, function (square) {
        //    if (square !== self)
        //        square.IsRelatedSelected(isActive);
        //});
    };

    // Css
    self.CssClass = ko.computed(function () {
        // css: { size4: $parents[1].Size() === 4, size9: $parents[1].Size() === 9, size16: $parents[1].Size() === 16, passiveSelected: IsPassiveSelected(), activeSelected: IsActiveSelected(), odd: $parent.IsOdd
        return 'squareItem ' + self.Group.Sudoku.CssSize() + (self.IsPassiveSelected() ? ' passiveSelected' : '') + (self.IsActiveSelected() ? ' activeSelected' : '') + (self.Group.IsOdd ? ' odd' : '');
    });

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
        self.IsSelected(event.type == 'mouseenter');

        // Find & toggle select related square as well
        var relatedSquare = data.Sudoku.FindSquareBySquareId(data.SquareId);
        relatedSquare.Value(event.type == 'mouseenter' ? self.HintValue : 0);
        relatedSquare.AssignType(event.type == 'mouseenter' ? 2 : 0);
        relatedSquare.TogglePassiveSelect(relatedSquare, event);
    }
}

function Availability(square) {
    var self = this;
    self.Square = square;
    self.Value = 0;
    self.IsAvailable = ko.observable(true);

    // css: { unavailable_self: !$parent.IsAvailable(), unavailable_group: !IsAvailable() }
    self.CssClass = ko.computed(function () {

        var cssClass = 'availabilityItem';

        if (!self.Square.IsAvailable())
            cssClass += ' unavailable_self';

        if (!self.IsAvailable())
            cssClass += ' unavailable_group';

        return cssClass;
    });
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

        if (callback !== null)
            callback(data);

    }).fail(function (jqXHR) { handleError(jqXHR); });
};

// Post to server
function postApi(apiUrl, postData, callback) {

    $.post(apiUrl, postData, function (data) {

        if (callback !== null)
            callback(data);

    }).fail(function (jqXHR) { handleError(jqXHR); });
};

// Loading message during ajax
$(function () {
    $(this).ajaxStart(function () {

        // Hide previous error message
        hideMessagePanel();

        $('#loadingMessagePanel').dialog('open');

    }).ajaxStop(function () {
        $('#loadingMessagePanel').dialog('close');
    })
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
