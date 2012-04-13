$(function () {

    //Init model
    initModels();

    /* Event handlers */

    //Load sudoku
    $("a.loadSudoku").live('click', function () {
        //Get the current id from UI
        var sudokuId = $(this).data('sudokuId');

        loadSudokuById(sudokuId);
    });

    //New sudoku
    $("a#newSudoku").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/newsudoku',
            cache: false, //?
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

                    /* TODO ?! */

                    //var validationResult = $.parseJSON(jqxhr.responseText);
                    //$.validator.unobtrusive.revalidate(form, validationResult);
                }
            }
        });
    });

    //Reset list
    $("a#resetList").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/reset',
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
    $("a#toggleReady").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/toggleready/' + detailsModel.Sudoku().SudokuId,
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
    $("a#toggleAutoSolve").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/toggleautosolve/' + detailsModel.Sudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //Update UI
                    detailsModel.Sudoku().AutoSolve = !detailsModel.Sudoku().AutoSolve;

                    if (detailsModel.Sudoku().AutoSolve) {
                        //Load it from server to get the updates
                        loadSudokuById(detailsModel.Sudoku().SudokuId);
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
    $("a#solve").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/solve/' + detailsModel.Sudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //Load it from the server
                    loadSudokuById(detailsModel.Sudoku().SudokuId);

                },
                400 /* BadRequest */: function (jqxhr) {

                    /* TODO ?! */

                    //var validationResult = $.parseJSON(jqxhr.responseText);
                    //$.validator.unobtrusive.revalidate(form, validationResult);
                }
            }
        });
    });

    //Square input focus + hover
    $("input.square").live('focus', function () { $(this).select(); });
    $("input.square").live({ mouseenter: function () { $(this).addClass('selected'); }, mouseleave: function () { $(this).removeClass('selected'); } });

    //Square input change
    $("input.square").live('change', function () {

        //Get the values
        var square = $(this);
        var squareId = square.data('squareId');
        var number = square.val();

        //Prepare post data
        var squareContainer = { SquareId: squareId, Number: number };
        var json = JSON.stringify(squareContainer);

        $.ajax({
            url: '/api/SudokuApi/updatesquare/' + detailsModel.Sudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            data: json,
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //If it it's "Ready", then it must use "user" class, instead of "initial"
                    //TODO Can we use more general approach - when it's "Ready", then all squares defualt class will be "user"?
                    if (detailsModel.Sudoku().Ready) {
                        square.attr('class', square.attr('class').replace('initial', 'user'));
                    }

                    //If it's "AutoSolve", load it from the server
                    if (detailsModel.Sudoku().AutoSolve) {
                        loadSudokuById(detailsModel.Sudoku().SudokuId);
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

    //Load the app
    loadApplication();
});

function initModels() {
    listModel = { SudokuList: ko.observableArray([]) };

    detailsModel = { Sudoku: ko.observable(null) };

    groupsModel = { Groups: ko.observableArray([]) };

    numbersModel = { Numbers: ko.observableArray([]) };

    potentialsModel = { Potentials: ko.observableArray([]) };
}

function loadApplication() {
    //Load sudoku list
    loadSudokuList();

    //Load default (first) sudoku
    loadSudokuById(1);
}

function loadSudokuList() {
    $.get('/api/SudokuApi/list', function (sudokuList) {
        listModel.SudokuList = ko.observableArray([]);
        listModel.SudokuList(sudokuList);
        ko.applyBindings(listModel, document.getElementById("listPanel"));
    });
}

function loadSudokuById(sudokuId) {
    //Get sudoku
    $.get('/api/SudokuApi/item/' + sudokuId, function (sudoku) {
        loadSudoku(sudoku);
    });
}

function loadSudoku(sudoku) {

    //Load sudoku
    detailsModel.Sudoku = ko.observable(null);
    detailsModel.Sudoku(sudoku);
    ko.applyBindings(detailsModel.Sudoku, document.getElementById("detailsPanel"));

    //Load groups
    loadGroups(sudoku.SudokuId);

    //Load numbers
    loadNumbers(sudoku.SudokuId);

    //Load potentials
    loadPotentials(sudoku.SudokuId);
}

function loadGroups(sudokuId) {

    $.get('/api/SudokuApi/squaretypegroups/' + sudokuId, function (groupList) {

        groupsModel.Groups = ko.observableArray([]);
        groupsModel.Groups(groupList);
        ko.applyBindings(groupsModel, document.getElementById("groupsPanel"));

        //Square group styling
        //TODO Is it possible to do it just once?
        $("div.groupItem:odd > .square").addClass('odd');

    });
}

function loadNumbers(sudokuId) {

    $.get('/api/SudokuApi/numbers/' + sudokuId, function (numberList) {

        numbersModel.Numbers = ko.observableArray([]);
        numbersModel.Numbers(numberList);
        ko.applyBindings(numbersModel, document.getElementById("numbersPanel"));

    });
}

function loadPotentials(sudokuId) {

    $.get('/api/SudokuApi/potentials/' + sudokuId, function (potentialList) {

        potentialsModel.Potentials = ko.observableArray([]);
        potentialsModel.Potentials(potentialList);
        ko.applyBindings(potentialsModel, document.getElementById("potentialsPanel"));

    });
}
