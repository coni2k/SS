function SudokuViewModel() {
    var self = this;

    // Basics
    self.SudokuId = ko.observable(0);
    self.Title = ko.observable('');
    self.Description = ko.observable('');
    self.Size = ko.observable(0);
    self.TotalSize = ko.computed(function () { return self.Size() * self.Size(); });
    self.SquareRootofSize = ko.computed(function () { return Math.sqrt(self.Size()); });
    self.SquaresLeft = ko.observable(0);
    self.SudokuList = ko.observableArray([]); // doesn't belong to sudoku class!
    self.Groups = ko.observableArray([]);
    self.NumberGroups = ko.observableArray([]);
    self.Hints = ko.observableArray([]);
    self.GroupNumberAvailabilities = ko.observableArray([]);
    self.SelectedSquare = ko.observable(null);
    self.SelectedNumber = ko.observable(null);
    self.Ready = ko.observable(false);
    self.AutoSolve = ko.observable(false);

    self.SetSelectedSquare = function (square) {

        // Remove the previous selected square
        if (self.SelectedSquare() !== null)
            self.SelectedSquare().SetActiveSelect(false);

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
            square.UpdateSquare(self.SelectedNumber().Value); // Else update the selected square
        }
    };

    // Selected number
    self.SetSelectedNumber = function (sudokuNumber) {

        // Remove the previous selected number
        if (self.SelectedNumber() !== null)
            self.SelectedNumber().SetActiveSelect(false);

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
            self.SelectedSquare().UpdateSquare(sudokuNumber.Value); // Else update the selected square
        }
    };

    self.LoadSudoku = function (sudoku) { loadSudoku(self, sudoku); }

    self.NewSudoku = function () {
        $('#newSudokuDialog').dialog({
            resizable: false,
            height: 275,
            width: 350,
            modal: true,
            buttons: {
                'Create': function () {

                    // Validate the form
                    var isValid = $('#newSudokuForm').validate().form();
                    if (!isValid)
                        return false;

                    // Close the dialog
                    $(this).dialog('close');

                    // Get the variables
                    var size = $('#newSudokuSize').val();
                    var title = $('#newSudokuTitle').val();
                    var description = $('#newSudokuDescription').val();

                    // Create a new sudoku
                    newSudoku(self, size, title, description);

                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    }

    self.ResetList = function () {
        $('#resetListDialog').dialog({
            resizable: false,
            height: 200,
            width: 450,
            modal: true,
            buttons: {
                'Reset list': function () {
                    $(this).dialog('close');
                    resetList(self);
                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    }

    // Ready
    self.ToggleReady = function () { toggleReady(self); }
    self.ReadyFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.Ready().toString());
    });

    // AutoSolve
    self.ToggleAutoSolve = function () { toggleAutoSolve(self); }
    self.AutoSolveFormatted = ko.computed(function () {
        return capitaliseFirstLetter(self.AutoSolve().toString());
    });

    // Solve
    self.Solve = function () { solve(self); };

    // Reset
    self.Resettable = ko.computed(function () {

        if (self.Ready()) {

            // If it's ready, check whether there are any squares that set by user, solver or hint
            return ko.utils.arrayFirst(self.Groups(), function (group) {
                return ko.utils.arrayFirst(group.Squares(), function (square) {
                    return square.AssignType() === 1 || square.AssignType() === 2 || square.AssignType() === 3;
                });
            }) !== null;
        }
        else {

            // If it's not ready, check whether there are any squares that have a value
            return ko.utils.arrayFirst(self.Groups(), function (group) {
                return ko.utils.arrayFirst(group.Squares(), function (square) {
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
            buttons: {
                'Reset': function () {
                    $(this).dialog('close');
                    reset(self);
                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });

    };

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

    // Filters
    // a. Find square by squareId
    self.FindSquareBySquareId = function (squareId) {

        var matchedSquare = null;

        // Loop through groups
        ko.utils.arrayFirst(self.Groups(), function (group) {

            // Loop through squares
            matchedSquare = ko.utils.arrayFirst(group.Squares(), function (square) {

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
            ko.utils.arrayForEach(group.Squares(), function (square) {

                // If it matches, add to the list
                if (square.Value() === number)
                    matchedSquares.push(square);
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
    // Visible = always!
}

function AvailabilityGrid(groups) {
    var self = this;
    self.Groups = ko.observableArray(groups);
    self.DisplayMode = 'availability';
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
    self.AssignType = ko.observable(0);

    self.ValueFormatted = ko.computed({
        read: function () {
            return self.Value() === 0 ? '' : self.Value();
        },
        write: function (value) {
            self.Value(value === '' ? 0 : value);
        }
    });

    self.RemoveValue = function () {
        self.UpdateSquare(0);
    };

    self.IsAvailable = ko.computed(function () { return self.Value() === 0; });

    self.IsUpdatable = ko.computed(function () { return !(self.Group.Model.Ready() && self.AssignType() === 0 && !self.IsAvailable()); });

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
        ko.utils.arrayForEach(self.Group.Squares(), function (square) {
            if (square !== self)
                square.IsRelatedSelected(isActive);
        });
    };

    // Related selected
    self.IsRelatedSelected = ko.observable(false);

    // Update (for click event)
    self.Update = function (data) { data.UpdateSquare(data.Value()); }

    // Update square
    self.UpdateSquare = function (newValue) {

        // Prepare the data
        var squareContainer = JSON.stringify({ SquareId: self.SquareId, Value: newValue });

        $.post(baseApiUrl + 'updatesquare/' + self.Group.Model.SudokuId(), squareContainer, function () {

            // Set the new values
            self.Value(newValue);
            self.AssignType(newValue === 0 ? 0 : self.Group.Model.Ready() ? 1 : 0);

            // Load details (if it's AutoSolve, load 'used squares' as well
            loadSudokuDetails(self.Group.Model, self.Group.Model.AutoSolve());

        }).fail(function (jqXHR) { handleError(jqXHR); });
    }
}

function SudokuNumber(model) {
    var self = this;
    self.Model = model;
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
        //ko.utils.arrayForEach(self.Group.Squares(), function (square) {
        //    if (square !== self)
        //        square.IsRelatedSelected(isActive);
        //});
    };
}

function Hint(model) {
    var self = this;
    self.Model = model;
    self.SquareId = 0;
    self.HintValue = 0;
    self.HintType = 0;
    self.IsSelected = ko.observable(false);
    self.ToggleSelect = function (data, event) {

        // Toggle select itself
        self.IsSelected(event.type == 'mouseenter');

        // Find & toggle select related square as well
        var relatedSquare = data.Model.FindSquareBySquareId(data.SquareId);
        relatedSquare.Value(event.type == 'mouseenter' ? self.HintValue : 0);
        relatedSquare.AssignType(event.type == 'mouseenter' ? 2 : 0);
        relatedSquare.TogglePassiveSelect(relatedSquare, event);
    }
}

function Availability() {
    var self = this;
    self.Value = 0;
    self.IsAvailable = ko.observable(true);
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

function loadSudokuList(model) {

    $.getJSON(baseApiUrl + 'list', function (sudokuList) {

        model.SudokuList([]);
        model.SudokuList(sudokuList);

        loadSudoku(model, model.SudokuList()[0]);

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function loadSudoku(model, sudoku) {

    // Determines whether the a new grid needs to be initialized or the existing one will be refreshed
    var initOrRefreshSudoku = model.Size() !== sudoku.Size;

    // Load sudoku
    model.SudokuId(sudoku.SudokuId);
    model.Title(sudoku.Title);
    model.Description(sudoku.Description);
    model.Size(sudoku.Size);
    model.SquaresLeft(sudoku.SquaresLeft);
    model.Ready(sudoku.Ready);
    model.AutoSolve(sudoku.AutoSolve);

    // Grid
    if (initOrRefreshSudoku)
        initSudoku(model);
    else
        refreshGrid(model);

    // Load details
    loadSudokuDetails(model);

    // Clear messages
    hideMessagePanel();
}

function loadSudokuDetails(model, refreshSquares) {
    // Optional refresh squares, default value is 'true'
    refreshSquares = (typeof refreshSquares === 'undefined') ? true : refreshSquares;

    // If refreshSquares flag is true, load used squares
    if (refreshSquares) {
        loadSquares(model);
    }

    // Load numbers
    loadNumbers(model);

    // Load hints
    loadHints(model);

    // Load availabilities
    loadAvailabilities(model);

    // Load availabilities
    // loadAvailabilities2(model);

    // Load group number availabilities
    // loadGroupNumberAvailabilities(model);

}

function initSudoku(model) {

    // Squares
    // Create an array for square type groups
    model.Groups([]);
    var size = model.Size();
    for (groupCounter = 1; groupCounter <= size; groupCounter++) {

        // Create group
        var group = new Group(model);
        group.GroupId(groupCounter);

        // Squares loop
        for (var squareCounter = 1; squareCounter <= size; squareCounter++) {

            // Create square
            var squareId = squareCounter + ((groupCounter - 1) * size);
            square = new Square(group, squareId);

            // Availability loop
            for (var availabilityCounter = 0; availabilityCounter < size; availabilityCounter++) {

                // Create availability item
                var availability = new Availability();
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

        model.Groups.push(group);
    }

    // Numbers
    // Group the numbers, to be able to display them nicely (see numbersPanel on default.html)
    model.NumberGroups([]);
    var sqrtSize = model.SquareRootofSize();
    for (groupCounter = 1; groupCounter <= sqrtSize; groupCounter++) {

        var numberGroup = new Object();
        numberGroup.Numbers = new Array();

        for (numberCounter = 1; numberCounter <= sqrtSize; numberCounter++) {

            var sudokuNumber = new SudokuNumber(model);
            sudokuNumber.Value = numberCounter + ((groupCounter - 1) * sqrtSize);
            numberGroup.Numbers.push(sudokuNumber);
        }

        model.NumberGroups.push(numberGroup);
    };

    // UI calculations;
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
            square.AssignType(0);
            ko.utils.arrayForEach(square.Availabilities(), function (availability) {
                availability.IsAvailable(true);
            });
        });
    });
}

function loadSquares(model) {

    $.getJSON(baseApiUrl + 'squares/' + model.SudokuId(), function (squareList) {

        $.each(squareList, function () {

            // Find the square
            var matchedSquare = model.FindSquareBySquareId(this.Id);

            // Update
            matchedSquare.Value(this.Number.Value);
            matchedSquare.AssignType(this.AssignType);
        });

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function loadNumbers(model) {

    $.getJSON(baseApiUrl + 'numbers/' + model.SudokuId(), function (numberList) {

        // Zero value (SquaresLeft on detailsPanel)
        var zeroNumber = numberList.splice(0, 1);
        model.SquaresLeft(zeroNumber[0].Count);

        $.each(numberList, function () {

            // Find the square
            var matchedNumber = model.FindNumberByNumberValue(this.Value);

            // Update
            matchedNumber.Count(this.Count);
        });

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function loadHints(model) {

    $.getJSON(baseApiUrl + 'hints/' + model.SudokuId(), function (hintList) {

        // Reset
        model.Hints([]);

        $.each(hintList, function () {

            // Create hint
            var hint = new Hint(model);

            hint.SquareId = this.Square.Id;
            hint.HintValue = this.Number.Value;
            hint.HintType = this.Type;

            model.Hints.push(hint);
        });

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function loadAvailabilities(model) {

    $.getJSON(baseApiUrl + 'availabilities/' + model.SudokuId(), function (availabilityList) {

        $.each(availabilityList, function () {

            var availabilityItem = this;

            // Number
            var number = availabilityItem.Number.Value;

            // Find the square
            var matchedSquare = model.FindSquareBySquareId(availabilityItem.SquareId);

            // Find the availability
            var matchedAvailability = ko.utils.arrayFirst(matchedSquare.Availabilities(), function (availability) {
                return availability.Value === number;
            });

            // Update
            matchedAvailability.IsAvailable(availabilityItem.IsAvailable);

        });

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

//function loadAvailabilities2(model) {

//    $.getJSON(baseApiUrl + 'availabilities2/' + model.SudokuId(), function (list) {

//        $.each(list, function () {

//            //Number
//            var number = this.Number;

//            //Find the square
//            var matchedSquare = model.FindSquareBySquareId(this.SquareId);

//            //Find the availability
//            var matched = ko.utils.arrayFirst(matchedSquare.Availabilities2(), function (availability2) {
//                return availability2.Value === number;
//            });

//            //Update
//            matched.IsAvailable(this.IsAvailable);

//        });

//    }).fail(function (jqXHR) { handleError(jqXHR); });
//}

function loadGroupNumberAvailabilities(model) {

    $.getJSON(baseApiUrl + 'groupnumberavailabilities/' + model.SudokuId(), function (list) {

        model.GroupNumberAvailabilities([]);

        $.each(list, function () {

            var groupNumberAvailability = new GroupNumberAvailability();
            groupNumberAvailability.GroupId = this.GroupId;
            groupNumberAvailability.Number = this.Number;
            groupNumberAvailability.Count = this.Count;
            model.GroupNumberAvailabilities.push(groupNumberAvailability);
        });

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function newSudoku(model, size, title, description) {

    // Prepare the data
    var sudokuContainer = JSON.stringify({ Size: size, Title: title, Description: description });

    $.post(baseApiUrl + 'newsudoku', sudokuContainer).done(function (newSudoku) {

        // Add the item to the list
        model.SudokuList.push(newSudoku);

        // Load the sudoku
        loadSudoku(model, newSudoku);

    }).fail(function (jqXHR) { handleError(jqXHR); });
};

function resetList(model) {

    $.post(baseApiUrl + 'resetlist').done(function () {

        // Load app again
        loadSudokuList(model);

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function toggleReady(model) {

    $.post(baseApiUrl + 'toggleready/' + model.SudokuId()).done(function () {

        // Toggle
        model.Ready(!model.Ready());

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function toggleAutoSolve(model) {

    $.post(baseApiUrl + 'toggleautosolve/' + model.SudokuId()).done(function () {

        // Update UI
        model.AutoSolve(!model.AutoSolve());

        // If autosolve, load details
        if (model.AutoSolve()) { loadSudokuDetails(model); }

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function solve(model) {

    $.post(baseApiUrl + 'solve/' + model.SudokuId()).done(function () {

        // Load details
        loadSudokuDetails(model);

    }).fail(function (jqXHR) { handleError(jqXHR); });
}

function reset(model) {

    $.post(baseApiUrl + 'reset/' + model.SudokuId()).done(function () {

        // Load details
        loadSudokuDetails(model);

    }).fail(function (jqXHR) { handleError(jqXHR); });

}

// TODO Should there be generic functions js file?
function capitaliseFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

// Loading panel
$(function () {
    $(this).ajaxStart(function () {

        // Hide previous error message
        hideMessagePanel();

        $('#loadingMessagePanel').dialog('open');

    }).ajaxStop(function () {

        $('#loadingMessagePanel').dialog('close');

    })
});

// Ajax error handling
function handleError(jqXHR) {

    // Get the message
    var validationResult = $.parseJSON(jqXHR.responseText).Message;

    // Show
    showMessagePanel(validationResult);
}

// Handle the delete key; remove selected square's value
$(document).keydown(function (e) {

    if (e.keyCode !== 46)
        return;

    if (sudokuViewModel.SelectedSquare() === null)
        return;

    var selectedSquare = sudokuViewModel.SelectedSquare();

    if (!selectedSquare.IsAvailable()) {
        selectedSquare.RemoveValue();
    }
});

// Handle the escape key; remove selected square
$(document).keyup(function (e) {
    
    if (e.keyCode !== 27)
        return;

    if (sudokuViewModel.SelectedSquare() !== null)
        sudokuViewModel.SetSelectedSquare(null);

    if (sudokuViewModel.SelectedNumber() !== null)
        sudokuViewModel.SetSelectedNumber(null);

});

// Panels
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
