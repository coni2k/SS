/* JSHint Configuration */
/*jshint jquery:true*/
/*global ko:true, Enumerable:true*/

(function (window, document, $, ko, Enumerable, History, undefined) {
    'use strict';

    // + Variables
    var
        /* WebAPI URLs */
        /* Root */
        apiUrlSudokuRoot = '/api/Sudoku/',

        /* Application level */
        apiUrlPostSudoku = apiUrlSudokuRoot + 'PostSudoku',
        apiUrlResetList = apiUrlSudokuRoot + 'ResetList',

        /* Sudoku level - get */
        apiUrlAllNumbers = function (sudokuId) { return apiUrlSudokuRoot + 'GetNumbers/' + sudokuId; },
        apiUrlUpdatedNumbers = function (sudokuId) { return apiUrlSudokuRoot + 'GetUpdatedNumbers/' + sudokuId; },

        apiUrlAllSquares = function (sudokuId) { return apiUrlSudokuRoot + 'GetSquares/' + sudokuId; },
        apiUrlUpdatedSquares = function (sudokuId) { return apiUrlSudokuRoot + 'GetUpdatedSquares/' + sudokuId; },

        apiUrlAllSquareAvailabilities = function (sudokuId) { return apiUrlSudokuRoot + 'GetSquareAvailabilities/' + sudokuId; },
        apiUrlUpdatedSquareAvailabilities = function (sudokuId) { return apiUrlSudokuRoot + 'GetUpdatedSquareAvailabilities/' + sudokuId; },

        apiUrlAllGroupNumberAvailabilities = function (sudokuId) { return apiUrlSudokuRoot + 'GetGroupNumberAvailabilities/' + sudokuId; },
        apiUrlUpdatedGroupNumberAvailabilities = function (sudokuId) { return apiUrlSudokuRoot + 'GetUpdatedGroupNumberAvailabilities/' + sudokuId; },

        apiUrlHints = function (sudokuId) { return apiUrlSudokuRoot + 'GetHints/' + sudokuId; },

        /* Sudoku level - post */
        apiUrlPutSquare = function (sudokuId, squareId) { return apiUrlSudokuRoot + 'PutSquare/' + sudokuId + '/' + squareId; },
        apiUrlToggleReady = function (sudokuId) { return apiUrlSudokuRoot + 'ToggleReady/' + sudokuId; },
        apiUrlToggleAutoSolve = function (sudokuId) { return apiUrlSudokuRoot + 'ToggleAutoSolve/' + sudokuId; },
        apiUrlSolve = function (sudokuId) { return apiUrlSudokuRoot + 'Solve/' + sudokuId; },
        apiUrlReset = function (sudokuId) { return apiUrlSudokuRoot + 'Reset/' + sudokuId; },

        /* Enums */
        AssignTypes = { 'Initial': 0, 'User': 1, 'Hint': 2, 'Solver': 3 },
        DataRequestTypes = { 'All': 0, 'Updated': 1 };

    // Bind knockout
    var sudokuViewModel = new SudokuViewModel();
    ko.applyBindings(sudokuViewModel);

    // + Objects
    function SudokuViewModel() {

        var self = this;

        self.Sudokus = ko.observableArray([]);
        self.NumberGroupsCache = [];
        self.SquareGroupsCache = [];
        self.GroupsCache = [];

        // TODO History.js enabled check ?!

        // History - bind to StateChange Event
        History.Adapter.bind(window, 'statechange', function () { // Note: We are using statechange instead of popstate
            self.LoadCurrentSudoku();
        });

        // Load list
        self.LoadSudokus = function () {

            // Not async.
            getData(apiUrlSudokuRoot, function (sudokuList) {

                self.Sudokus([]);

                Enumerable.From(sudokuList).ForEach(function (sudokuItem) {

                    var sudoku = new Sudoku(self, sudokuItem.Size);
                    sudoku.SudokuId(sudokuItem.SudokuId),
                    sudoku.Title(sudokuItem.Title);
                    sudoku.Description(sudokuItem.Description);
                    sudoku.SquaresLeft(sudokuItem.SquaresLeft);
                    sudoku.Ready(sudokuItem.Ready);
                    sudoku.AutoSolve(sudokuItem.AutoSolve);

                    self.Sudokus.push(sudoku);

                });

            }, false);
        };

        self.NavigateEvent = function (sudokuItem, event) {

            self.Navigate(sudokuItem, true);

            event.preventDefault();
            return false;
        }

        self.Navigate = function (sudokuItem, isPush) {

            if (isPush) {
                History.pushState(null, null, sudokuItem.Url());
            }
            else {
                History.replaceState(null, null, sudokuItem.Url());
            }
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
                        var sudokuDto = JSON.stringify({ Title: title, Description: description, Size: size });

                        // Post
                        postData(apiUrlPostSudoku, sudokuDto, function (newSudokuItem) {

                            // Add the item to the list
                            var newSudoku = new Sudoku(self, newSudokuItem.Size);
                            newSudoku.SudokuId(newSudokuItem.SudokuId);
                            newSudoku.Title(newSudokuItem.Title);
                            newSudoku.Description(newSudokuItem.Description);
                            newSudoku.SquaresLeft(newSudokuItem.SquaresLeft);
                            newSudoku.Ready(newSudokuItem.Ready);
                            newSudoku.AutoSolve(newSudokuItem.AutoSolve);

                            self.Sudokus.push(newSudoku);
                            self.Navigate(newSudoku, true);

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

                        // Post reset + load
                        postData(apiUrlResetList, null, function () {
                            self.LoadSudokus(); // Async false
                            self.LoadCurrentSudoku();
                        });

                    },
                    Cancel: function () {
                        $(this).dialog('close');
                    }
                }
            });
        };

        // Panels
        $("#loadingMessagePanel").dialog({
            dialogClass: 'ui-dialog-notitlebar',
            resizable: false,
            height: 70,
            width: 250,
            modal: true
        });

        // Cache functions: Since calculating the squares + numbers take time, the results are cached in these arrays
        // a. Numbers
        self.GetNumberGroups = function (size) {

            // Search the groups in the cache
            var cacheItem = Enumerable.From(self.NumberGroupsCache).SingleOrDefault(null, function (item) {
                return item.Size === size;
            });

            // If there is none, create
            if (cacheItem === null) {

                var numberGroups = [];
                var squareRootofSize = Math.sqrt(size);

                // Group the numbers, to be able to display them nicely (see numbersPanel on default.aspx)
                for (var groupCounter = 1; groupCounter <= squareRootofSize; groupCounter++) {

                    var numberGroup = new SudokuNumberGroup(groupCounter, size);

                    for (var numberCounter = 1; numberCounter <= squareRootofSize; numberCounter++) {

                        // Calculate the value
                        var value = numberCounter + ((groupCounter - 1) * squareRootofSize);

                        // New number
                        var sudokuNumber = new SudokuNumber(numberGroup, value, size);

                        // Add the number
                        numberGroup.Numbers.push(sudokuNumber);
                    }

                    numberGroups.push(numberGroup);
                }

                cacheItem = new NumberGroupsCacheItem(size, numberGroups);

                self.NumberGroupsCache.push(cacheItem);
            }

            return cacheItem.NumberGroups;
        }

        // b. Squares
        self.GetSquareGroups = function (size) {

            var cacheItem = Enumerable.From(self.SquareGroupsCache).SingleOrDefault(null, function (item) {
                return item.Size === size;
            });

            if (cacheItem === null) {

                var squareGroups = [];
                var cssSize = 'size' + size.toString(10);

                // Square groups loop
                for (var groupCounter = 1; groupCounter <= size; groupCounter++) {

                    // Create group
                    var group = new SquareGroup(groupCounter, size);

                    // Squares loop
                    for (var squareCounter = 1; squareCounter <= size; squareCounter++) {

                        // Create square
                        var squareId = squareCounter + ((groupCounter - 1) * size);
                        var square = new Square(squareId, group, size);

                        // Availability loop
                        for (var availabilityCounter = 0; availabilityCounter < size; availabilityCounter++) {

                            // Create availability item
                            var availability = new SquareAvailability(square);
                            availability.Value = availabilityCounter + 1;
                            square.Availabilities.push(availability);
                        }

                        group.Squares.push(square);
                    }

                    squareGroups.push(group);
                }

                cacheItem = new SquareGroupCacheItem(size, squareGroups);

                self.SquareGroupsCache.push(cacheItem);
            }

            return cacheItem.SquareGroups;
        };

        // b. Group number availabilities
        self.GetGroups = function (size) {

            var cacheItem = Enumerable.From(self.GroupsCache).SingleOrDefault(null, function (item) {
                return item.Size === size;
            });

            if (cacheItem === null) {

                var groups = [];
                var cssSize = 'size' + size.toString(10);

                // Square groups loop
                for (var groupCounter = 1; groupCounter <= size; groupCounter++) {

                    // Create group
                    var group = new Group(groupCounter, size);

                    // Squares loop
                    for (var squareCounter = 1; squareCounter <= size; squareCounter++) {

                        // Create square
                        var groupNumber = new GroupNumber(squareCounter, group, size);

                        // Availability loop
                        for (var availabilityCounter = 0; availabilityCounter < size; availabilityCounter++) {

                            // Create availability item
                            var availability = new GroupNumberAvailability(squareId);
                            var squareId = (availabilityCounter + 1) + ((groupCounter - 1) * size);
                            availability.SquareId = squareId;
                            groupNumber.GroupNumberAvailabilities.push(availability);
                        }

                        group.GroupNumbers.push(groupNumber);
                    }

                    groups.push(group);
                }

                cacheItem = new GroupCacheItem(size, groups);

                self.GroupsCache.push(cacheItem);
            }

            return cacheItem.Groups;
        };

        // Load sudoku list + current sudoku (from current state)
        self.SelectedSudoku = ko.observable(new Sudoku(self));
        self.HasSelectedSudoku = ko.computed(function () { return self.SelectedSudoku().Size > 0; });

        // Load sudoku (from current history state)
        // self.LoadCurrentSudoku = function (currentState) {
        self.LoadCurrentSudoku = function (currentState) {

            // Hide previous messages
            hideMessagePanel();

            // Default sudoku
            var currentSudokuId = 0;

            var currentState = History.getState();

            // If there is a state (can be null if it's the first load or reset list)
            if (currentState !== null) {

                // Remove the host part
                var url = currentState.url.replace(History.getRootUrl(), '').replace('default.aspx', '');

                // Try to get sudoku id
                if (url !== '') {

                    var currentContentId = url.split('/')[0];

                    // If it's not sudoku page, don't handle
                    if (currentContentId !== 'sudoku')
                        return;

                    if (url.indexOf('/') > 0) {
                        currentSudokuId = parseInt(url.split('/')[1], 10);
                    }
                }
            }

            // Search for the item
            var selectedSudoku = Enumerable.From(self.Sudokus()).SingleOrDefault(null, function (sudoku) {
                return sudoku.SudokuId() === currentSudokuId;
            });

            // There is no sudoku with the current id, get the first one as the default
            // Navigate to 404 ?
            if (selectedSudoku === null) {
                self.Navigate(self.Sudokus()[0], true);
                return;
            };

            // Clear the selections
            self.SelectedSudoku().ClearSelectedSquare();
            self.SelectedSudoku().ClearSelectedNumber();

            // Load the selected sudoku
            self.SelectedSudoku(selectedSudoku);

            // Page title + header
            document.title = 'Sudoku Solver - Sudoku - ' + self.SelectedSudoku().SudokuId();

            // Load details
            self.SelectedSudoku().LoadDetails(DataRequestTypes.All);
        }

        self.LoadSudokus(); // Async false
        // self.LoadCurrentSudoku(History.getState());
        self.LoadCurrentSudoku();
    }

    function NumberGroupsCacheItem(size, numberGroups)
    {
        var self = this;

        self.Size = size;
        self.NumberGroups = numberGroups;
    }

    function SquareGroupCacheItem(size, squareGroups)
    {
        var self = this;

        self.Size = size;
        self.SquareGroups = squareGroups;
    }

    function GroupCacheItem(size, groups) {
        var self = this;

        self.Size = size;
        self.Groups = groups;
    }

    // Sudoku
    function Sudoku(sudokuViewModel, size) {
        size = (typeof size === 'undefined') ? 0 : size;

        var self = this;
        self.Model = sudokuViewModel;

        // Variables
        self.SudokuId = ko.observable(0);
        self.Title = ko.observable('');
        self.Description = ko.observable('');
        self.Size = size;
        self.TotalSize = self.Size * self.Size;
        self.SquareRootofSize = Math.sqrt(self.Size);

        // Dynamic properties
        self.Url = ko.computed(function () {
            return '/sudoku/' + self.SudokuId().toString();
        });
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
        self.NumberGroups = self.Model.GetNumberGroups(self.Size);
        self.SquareGroups = self.Model.GetSquareGroups(self.Size);
        self.Groups = self.Model.GetGroups(self.Size);
        self.Hints = ko.observableArray([]);

        // Grids;
        // a. Value grid
        self.ValueGrid = new ValueGrid(self.SquareGroups);
        self.ValueGridCss = 'panel gridPanel size' + self.Size;

        // b. Square availability grid
        self.SquareAvailabilityGrid = new SquareAvailabilityGrid(self.SquareGroups);
        self.SquareAvailabilityGridCss = ko.computed(function () { return 'panel gridPanel size' + self.Size + (self.SquareAvailabilityGrid.Visible() ? '' : ' hide'); });
        self.ToggleDisplaySquareAvailabilities = function () {
            self.SquareAvailabilityGrid.Visible(!self.SquareAvailabilityGrid.Visible());
        };
        self.DisplaySquareAvailabilitiesFormatted = ko.computed(function () {
            return capitaliseFirstLetter(self.SquareAvailabilityGrid.Visible().toString());
        });

        // c. Group number availability grid
        self.GroupNumberAvailabilityGrid = new GroupNumberAvailabilityGrid(self.Groups);
        self.GroupNumberAvailabilityGridCss = ko.computed(function () { return 'panel gridPanel size' + self.Size + (self.GroupNumberAvailabilityGrid.Visible() ? '' : ' hide'); });
        self.ToggleDisplayGroupNumberAvailabilities = function () {
            self.GroupNumberAvailabilityGrid.Visible(!self.GroupNumberAvailabilityGrid.Visible());
        };
        self.DisplayGroupNumberAvailabilitiesFormatted = ko.computed(function () {
            return capitaliseFirstLetter(self.GroupNumberAvailabilityGrid.Visible().toString());
        });

        // d. Id grid
        self.IdGrid = new IDGrid(self.SquareGroups);
        self.IdGridCss = ko.computed(function () { return 'panel gridPanel size' + self.Size + (self.IdGrid.Visible() ? '' : ' hide'); });
        self.ToggleDisplayIDs = function () {
            self.IdGrid.Visible(!self.IdGrid.Visible());
        };
        self.DisplayIDsFormatted = ko.computed(function () {
            return capitaliseFirstLetter(self.IdGrid.Visible().toString());
        });

        // Filters
        // a. Find square by squareId
        self.FindSquareBySquareId = function (squareId) {

            var matchedSquare = null;

            // Loop through groups
            Enumerable.From(self.SquareGroups).ForEach(function (group) {

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

            var matchedSquares = [];

            // Loop through groups
            Enumerable.From(self.SquareGroups).ForEach(function (squareGroup) {

                // Search for the squares with the number
                var matchedSquare = Enumerable.From(squareGroup.Squares).SingleOrDefault(null, function (square) {
                    return square.SudokuNumber().Value === number;
                });

                // If there is, add it to the list
                if (matchedSquare !== null) {
                    matchedSquares.push(matchedSquare);
                }
            });

            return matchedSquares;
        };

        // c. Find number by value
        self.FindNumberByValue = function (value) {

            var matchedNumber = null;
            
            // Loop through groups
            Enumerable.From(self.NumberGroups).ForEach(function (group) {

                // Search fro the number with value
                matchedNumber = Enumerable.From(group.Numbers).SingleOrDefault(null, function (sudokuNumber) {
                    return sudokuNumber.Value === value;
                });

                // If found, break the loop
                if (matchedNumber !== null) {
                    return false;
                }
            });

            return matchedNumber;
        };

        // Filters
        // d. Find group number by number value
        self.FindGroupNumberAvailability = function (groupId, numberValue, squareId) {

            // alert('ok 1');
            var matchedGroup = Enumerable.From(self.Groups).Single(function (group) {
                return group.GroupId === groupId;
            });

            // alert('ok 2');
            var matchedGroupNumber = Enumerable.From(matchedGroup.GroupNumbers).Single(function (groupNumber) {
                return groupNumber.NumberValue === numberValue;
            });

            // alert('ok 3');
            var matchedAvailability = Enumerable.From(matchedGroupNumber.GroupNumberAvailabilities).Single(function (availability) {
                // console.log(availability.SquareId);
                return availability.SquareId === squareId;
            });

            return matchedAvailability;
        };

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

        self.ClearSelectedSquare = function () {
            self.SetSelectedSquare(null);
        }

        // Selected number
        self.SetSelectedNumber = function (sudokuNumber) {

            // Remove the selection from previous number
            if (self.SelectedNumber() !== null) {
                self.SelectedNumber().SetActiveSelect(false);

                // Remove the selection from its related squares
                Enumerable.From(self.FindSquareByNumber(self.SelectedNumber().Value)).ForEach(function (square) {
                    square.IsRelatedSelected(false);
                });
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

                // Select its related squares
                Enumerable.From(self.FindSquareByNumber(self.SelectedNumber().Value)).ForEach(function (square) {
                    square.IsRelatedSelected(true);
                });
            }
            else {
                self.UpdateSelectedSquare(self.SelectedSquare(), sudokuNumber.Value);
            }
        };

        self.ClearSelectedNumber = function () {
            self.SetSelectedNumber(null);
        };

        // Update selected square
        self.UpdateSelectedSquare = function (square, newValue) {

            // Prepare dto
            var squareDto = JSON.stringify({ SudokuId: self.SudokuId(), SquareId: square.SquareId, Value: newValue });

            // Put + load
            putData(apiUrlPutSquare(self.SudokuId(), square.SquareId), squareDto, function () { self.LoadDetails(DataRequestTypes.Updated); });
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
            if (self.Ready() && self.SelectedSquare().AssignType() === AssignTypes.Initial) {
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
            postData(apiUrlToggleReady(self.SudokuId()), null, function () {

                // Update client
                self.Ready(!self.Ready());
            });
        };

        // Toggle auto solve
        self.ToggleAutoSolve = function () {
            postData(apiUrlToggleAutoSolve(self.SudokuId()), null, function () {

                // Update client
                self.AutoSolve(!self.AutoSolve());

                // If autosolve, load details
                if (self.AutoSolve()) {
                    self.LoadDetails(DataRequestTypes.Updated);
                }
            });
        };

        // Solve
        self.Solve = function () {
            postData(apiUrlSolve(self.SudokuId()), null, function () { self.LoadDetails(DataRequestTypes.Updated); });
        };

        // Reset
        self.Resettable = ko.computed(function () {
            return Enumerable.From(self.SquareGroups).Any(function (squareGroup) {
                return Enumerable.From(squareGroup.Squares).Any(function (square) {
                    if (self.Ready()) {
                        // If it's ready, check whether there are any squares that set by user, solver or hint
                        return square.AssignType() === AssignTypes.User || square.AssignType() === AssignTypes.Hint || square.AssignType() === AssignTypes.Solver;
                    }
                    else {
                        // If it's not ready, check whether there are any squares that have a value
                        return square.AssignType() === AssignTypes.Initial && square.SudokuNumber().Value !== 0;
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
                        postData(apiUrlReset(self.SudokuId()), null, function () { self.LoadDetails(DataRequestTypes.All); });
                    },
                    Cancel: function () {
                        $(this).dialog('close');
                    }
                }
            });
        };

        // Load methods
        self.LoadDetails = function (type) {

            // Load numbers
            self.LoadNumbers(type);

            // Load squares
            self.LoadSquares(type);

            // Load square availabilities
            self.LoadSquareAvailabilities(type);

            // Load group number availabilities
            self.LoadGroupNumberAvailabilities(type);

            // Load hints
            self.LoadHints();
        };

        self.LoadSquares = function (type) {

            // Determine the url based on the type
            var apiUrl = type === DataRequestTypes.All
                ? apiUrlAllSquares(self.SudokuId())
                : apiUrlUpdatedSquares(self.SudokuId());

            // Get the squares
            getData(apiUrl, function (squareList) {

                Enumerable.From(squareList).ForEach(function (squareItem) {

                    var square = self.FindSquareBySquareId(squareItem.SquareId);
                    var sudokuNumber = new SudokuNumber(null, squareItem.Value, 0);
                    square.SudokuNumber(sudokuNumber);
                    square.AssignType(squareItem.AssignType);
                });
            });
        };

        self.LoadNumbers = function (type) {

            // Determine the url based on the type
            var apiUrl = type === DataRequestTypes.All
                ? apiUrlAllNumbers(self.SudokuId())
                : apiUrlUpdatedNumbers(self.SudokuId());

            // Get the numbers
            getData(apiUrl, function (numberList) {

                // Zero value (SquaresLeft on detailsPanel)
                var zeroNumber = numberList.splice(0, 1);

                // TODO This is already assigned ?!
                self.SquaresLeft(zeroNumber[0].Count);

                // Count of other numbers
                Enumerable.From(numberList).ForEach(function (numberItem) {

                    Enumerable.From(self.NumberGroups).ForEach(function (numberGroup) {

                        Enumerable.From(numberGroup.Numbers).ForEach(function (number) {

                            if (number.Value === numberItem.Value) {
                                number.Count(numberItem.Count);
                                return;
                            }
                        });
                    });
                });
            });
        };

        self.LoadSquareAvailabilities = function (type) {

            // Determine the url based on the type
            var apiUrl = type === DataRequestTypes.All
                ? apiUrlAllSquareAvailabilities(self.SudokuId())
                : apiUrlUpdatedSquareAvailabilities(self.SudokuId());

            // Get the availabilities
            getData(apiUrl, function (availabilityList) {

                Enumerable.From(availabilityList).ForEach(function (availabilityItem) {

                    // Get the square
                    var square = self.FindSquareBySquareId(availabilityItem.SquareId);

                    // Get the availability
                    var availability = Enumerable.From(square.Availabilities).Single(function (availability) {
                        return availability.Value === availabilityItem.Value;
                    });

                    // Set IsAvailable
                    availability.IsAvailable(availabilityItem.IsAvailable);

                });
            });
        };

        self.LoadGroupNumberAvailabilities = function (type) {

            // Determine the url based on the type
            var apiUrl = type === DataRequestTypes.All
                ? apiUrlAllGroupNumberAvailabilities(self.SudokuId())
                : apiUrlUpdatedGroupNumberAvailabilities(self.SudokuId());
            
            // Get the availabilities
            getData(apiUrl, function (availabilityList) {

                Enumerable.From(availabilityList).ForEach(function (availabilityItem) {

                    var availability = self.FindGroupNumberAvailability(availabilityItem.GroupId, availabilityItem.SudokuNumber, availabilityItem.SquareId);

                    availability.IsAvailable(availabilityItem.IsAvailable);

                });
            });
        };

        self.LoadHints = function () {

            // Get the hints from the server
            getData(apiUrlHints(self.SudokuId()), function (hintList) {

                // Reset the current list
                self.Hints([]);

                // Add the new hints to the list
                Enumerable.From(hintList).ForEach(function (hintItem) {

                    // Create hint
                    var hint = new Hint(self);

                    // var sudokuNumber = self.FindNumberByValue(hintItem.Number.Value);
                    var sudokuNumber = new SudokuNumber(null, hintItem.Value, 0);

                    hint.SquareId = hintItem.SquareId;
                    hint.HintValue(sudokuNumber);
                    hint.HintType = hintItem.Type;

                    self.Hints.push(hint);
                });
            });
        };
    }

    function ValueGrid(squareGroups) {
        var self = this;
        self.SquareGroups = squareGroups;
        self.Template = 'squareValueTemplate';
        // Visible = always!
    }

    function SquareAvailabilityGrid(squareGroups) {
        var self = this;
        self.SquareGroups = squareGroups;
        self.Template = 'squareAvailabilitiesTemplate';
        self.Visible = ko.observable(true);
    }

    function GroupNumberAvailabilityGrid(groups) {
        var self = this;
        self.Groups = groups;
        self.Template = 'groupNumberAvailabilitiesTemplate';
        self.Visible = ko.observable(true);
    }

    function IDGrid(squareGroups) {
        var self = this;
        self.SquareGroups = squareGroups;
        self.Template = 'squareIdTemplate';
        self.Visible = ko.observable(false);
    }

    function SquareGroup(groupId, sudokuSize) {
        var self = this;
        self.GroupId = groupId;
        self.Squares = [];
        self.IsOdd = (self.GroupId % 2 === 0);
        self.CssClass = 'groupItem size' + sudokuSize.toString(10);
    }

    function Square(squareId, group, sudokuSize) {
        var self = this;
        self.SquareId = squareId;
        self.Group = group;
        // self.SudokuNumber = ko.observable(new SudokuNumber(null, 0, 0));
        self.SudokuNumber = ko.observable(null);
        var zeroNumber = new SudokuNumber(null, 0, 0);
        self.SudokuNumber(zeroNumber); // = ko.observable(new SudokuNumber(null, 0, 0));
        self.AssignType = ko.observable(AssignTypes.Initial);

        self.IsAvailable = ko.computed(function () {
            return self.SudokuNumber().Value === 0;
        });

        self.ValueFormatted = ko.computed(function () {
            return self.IsAvailable() ? '&nbsp;' : self.SudokuNumber().Value;
        });

        // self.IsUpdateable = ko.computed(function () { return !(self.Group.Sudoku.Ready() && self.AssignType() === 0 && !self.IsAvailable()); });

        self.Availabilities = [];

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
        self.CssAssignType = ko.computed(function () {
            switch (self.AssignType()) {
                case AssignTypes.Initial:
                    return ' initial';
                case AssignTypes.User:
                    return ' user';
                case AssignTypes.Hint:
                    return ' hint';
                case AssignTypes.Solver:
                    return ' solver';
            }
        });

        self.CssClassComputed = ko.computed(function () {
            return 'squareItem '
                + 'size' + sudokuSize.toString(10)
                + self.CssAssignType()
                + (self.IsPassiveSelected() ? ' passiveSelected' : '')
                + (self.IsActiveSelected() ? ' activeSelected' : '')
                + (self.IsRelatedSelected() ? ' relatedSelected' : '')
                + (self.Group.IsOdd ? ' odd' : '');
        });

        // This is an alternative to CssClassComputed; in this way, this part is static and the rest is on html (to reduce the length of the calculated part)
        // self.CssClass = 'squareItem ' + self.Group.Sudoku.CssSize;
    }

    function SudokuNumberGroup(numberGroupId, size) {
        var self = this;
        self.NumberGroupId = numberGroupId;
        self.Numbers = [];
        self.IsOdd = (self.NumberGroupId % 2 === 0);
        self.CssClass = 'groupItem size' + size.toString(10);
    }

    function SudokuNumber(group, value, size) {
        var self = this;
        self.Group = group;
        self.Value = value;
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

        };

        // Css
        self.CssClassComputed = ko.computed(function () {
            return 'squareItem '
                + (self.Group ? 'size' + size.toString(10) : '') // !
                + (self.IsPassiveSelected() ? ' passiveSelected' : '')
                + (self.IsActiveSelected() ? ' activeSelected' : '')
                + (self.Group && self.Group.IsOdd ? ' odd' : '');
        });

        // This is an alternative to CssClassComputed; in this way, this part is static and the rest is on html (to reduce the length of the calculated part)
        // self.CssClass = 'squareItem ' + self.Group.Sudoku.CssSize;
    }

    function Hint(sudoku) {

        var self = this;

        self.Sudoku = sudoku;
        self.SquareId = 0;
        self.HintValue = ko.observable(null);

        var zeroNumber = new SudokuNumber(null, 0, 0);
        self.HintValue(zeroNumber);

        self.IsAvailable = ko.computed(function () {
            return self.HintValue().Value === 0;
        });

        self.ValueFormatted = ko.computed(function () {
            return self.IsAvailable() ? '&nbsp;' : self.HintValue().Value;
        });

        self.HintType = 0;
        self.IsSelected = ko.observable(false);
        self.ToggleSelect = function (data, event) {

            // Toggle select itself
            self.IsSelected(event.type === 'mouseenter');

            // Find & toggle select related square as well
            var relatedSquare = self.Sudoku.FindSquareBySquareId(data.SquareId);

            var zeroNumber = new SudokuNumber(null, 0, 0);

            var newNumber = event.type === 'mouseenter' ? self.HintValue() : zeroNumber;

            relatedSquare.SudokuNumber(newNumber);

            relatedSquare.AssignType(event.type === 'mouseenter' ? AssignTypes.Hint : AssignTypes.Initial);

            relatedSquare.TogglePassiveSelect(relatedSquare, event);
        };
    }

    function SquareAvailability(square) {
        var self = this;
        self.Square = square;
        self.Value = 0;
        self.IsAvailable = ko.observable(true);

        self.CssClassComputed = ko.computed(function () {
            return 'availabilityItem '
                + (!self.Square.IsAvailable() ? 'unavailable_self' : '')
                + (!self.IsAvailable() ? ' unavailable_group' : '');
        });
    }

    function Group(groupId, sudokuSize) {
        var self = this;
        self.GroupId = groupId;
        self.GroupNumbers = [];
        self.IsOdd = (self.GroupId % 2 === 0);
        self.CssClass = 'groupItem size' + sudokuSize.toString(10);
    }

    function GroupNumber(numberValue, group, sudokuSize) {
        var self = this;

        self.Group = group;
        self.NumberValue = numberValue;
        self.GroupNumberAvailabilities = [];

        self.CssClassComputed = ko.computed(function () {
            return 'squareItem '
                + 'size' + sudokuSize.toString(10)
                + (self.Group.IsOdd ? ' odd' : '');
        });
    }

    function GroupNumberAvailability(squareId) {
        var self = this;

        self.SquareId = squareId;
        self.IsAvailable = ko.observable(true);

        self.CssClassComputed = ko.computed(function () {
            return 'availabilityItem '
                + (!self.IsAvailable() ? ' unavailable_group' : '');
        });
    }

    // Get data from server
    function getData(apiUrl, callback, isAsync) {
        isAsync = (typeof isAsync === 'undefined') ? true : isAsync;

        $.ajax({
            url: apiUrl,
            async: isAsync,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        }).done(callback).fail(function (jqXHR) { handleError(jqXHR); });

    }

    // Post data to server
    function postData(apiUrl, postData, callback) {

        $.ajax({
            type: 'POST',
            url: apiUrl,
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {

            // Hide the previous error messages
            hideMessagePanel();

            // Callback func.
            callback(data);

        }).fail(function (jqXHR) { handleError(jqXHR); });
    }

    // Put data to server
    function putData(apiUrl, postData, callback) {

        $.ajax({
            type: 'PUT',
            url: apiUrl,
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {

            // Hide the previous error messages
            hideMessagePanel();

            // Callback func.
            callback(data);

        }).fail(function (jqXHR) { handleError(jqXHR); });
    }

    function handleError(jqXHR) {

        // Get the message
        var validationResult = $.parseJSON(jqXHR.responseText).Message;

        // Show
        showMessagePanel(validationResult);
    }

    // TODO Handle arrow keys?
    // TODO Make this as function of sudoku object?
    $(document).keydown(function (e) {

        switch (e.keyCode) {
            case 37: // Left
                {
                    break;
                }
            case 38: // Up
                {
                    break;
                }
            case 39: // Right
                {
                    break;
                }
            case 40: // Down
                {
                    break;
                }
        }
    });

    // Handle the delete key; remove selected square's value
    $(document).keydown(function (e) {

        if (e.keyCode !== 46) {
            return;
        }

        if (sudokuViewModel.SelectedSudoku().SelectedSquare() === null) {
            return;
        }

        sudokuViewModel.SelectedSudoku().RemoveSelectedSquareValue();
    });

    // Handle the escape key; remove selected square
    $(document).keyup(function (e) {

        if (e.keyCode !== 27) {
            return;
        }

        if (sudokuViewModel.SelectedSudoku().SelectedSquare() !== null) {
            sudokuViewModel.SelectedSudoku().ClearSelectedSquare();
        }

        if (sudokuViewModel.SelectedSudoku().SelectedNumber() !== null) {
            sudokuViewModel.SelectedSudoku().ClearSelectedNumber();
        }

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

    function capitaliseFirstLetter(string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }

})(window, window.document, window.jQuery, window.ko, window.Enumerable, window.History);
