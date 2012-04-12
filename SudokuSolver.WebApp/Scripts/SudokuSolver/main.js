$(function () {

    //Init model
    initModel();

    /* Event handlers */

    //Load sudoku
    $("a.load").live('click', function () {
        //Get the current id from UI
        var sudokuId = $(this).data('sudokuId');

        loadSudokuById(sudokuId);
    });

    //New sudoku
    $("a.newSudoku").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/newsudoku',
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                201 /*Created*/: function (data) {

                    //Load list again
                    //Is it the correct way of doing it, how about bindings?
                    loadSudokuList();

                    //Load sudoku?
                    loadSudoku(data);
                },
                400 /* BadRequest */: function (jqxhr) {

                    /* TODO ?! */

                    //var validationResult = $.parseJSON(jqxhr.responseText);
                    //$.validator.unobtrusive.revalidate(form, validationResult);
                }
            }
        });
    });

    //Reset samples
    $("a.resetSamples").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/resetsamples',
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //Load list again
                    //Is it the correct way of doing it, how about bindings?
                    //loadSudokuList();

                    //Load default (first) sudoku
                    //loadSudokuById(1);

                    loadInitialData();
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
    $("a.toggleReady").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/toggleready/' + viewModel.CurrentSudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //Update UI
                    viewModel.CurrentSudoku().Ready = !viewModel.CurrentSudoku().Ready;
                    ko.applyBindings(viewModel);
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
    $("a.toggleAutoSolve").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/toggleautosolve/' + viewModel.CurrentSudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //Update UI
                    //viewModel.CurrentSudoku().AutoSolve = !viewModel.CurrentSudoku().AutoSolve;

                    //Load sudoku?
                    loadSudokuById(viewModel.CurrentSudoku().SudokuId);
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
    $("a.solve").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/solve/' + viewModel.CurrentSudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //Load it again
                    //Is it necessary? how about bindings?
                    loadSudokuById(viewModel.CurrentSudoku().SudokuId);

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
    $("input.square").live('focus', function () { $(this).select(); } );
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
            url: '/api/SudokuApi/updatesquare/' + viewModel.CurrentSudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            data: json,
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*OK*/: function (data) {

                    //If it it's "Ready", then it must use "user" class, instead of "initial"
                    //TODO Can we use more general approach - when it's "Ready", then all squares defualt class will be "user"?
                    if (viewModel.CurrentSudoku().Ready) {
                        square.attr('class', square.attr('class').replace('initial', 'user'));
                    }

                    //If it's "AutoSolve", load it from the server
                    if (viewModel.CurrentSudoku().AutoSolve) {
                        //Load it again
                        //Is it necessary? how about bindings?
                        loadSudokuById(viewModel.CurrentSudoku().SudokuId);
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

    //Load sudoku list
    //loadSudokuList();

    //Load default (first) sudoku
    //loadSudokuById(1);

    //TODO !!!!
    loadInitialData();
});

function initModel()
{
    //Init model
    viewModel = {
        SudokuList: ko.observableArray([]),
        CurrentSudoku: ko.observable(null),
        CurrentHorizontalGroupList: ko.observableArray([])
    };
}

function loadSudokuList()
{
    viewModel.SudokuList = ko.observableArray([]);

    $.get('/api/SudokuApi/list', function (sudokuList) {
        viewModel.SudokuList(sudokuList);

        ko.applyBindings(viewModel);
    });
}

function loadSudokuById(sudokuId)
{
    //Get sudoku
    $.get('/api/SudokuApi/item/' + sudokuId, function (sudoku) {
        loadSudoku(sudoku);
    });
}

function loadSudoku(sudoku)
{
    viewModel.CurrentSudoku = ko.observable(null);
    viewModel.CurrentHorizontalGroupList = ko.observableArray([]);

    //Load sudoku
    viewModel.CurrentSudoku(sudoku);
    //ko.applyBindings(viewModel);

    //Get the squares!
    $.get('/api/SudokuApi/squaretypegroups/' + sudoku.SudokuId, function (list) {
        viewModel.CurrentHorizontalGroupList(list);
        //viewModel.CurrentSudoku.Groups(list);

        ko.applyBindings(viewModel);

        //Square group styling
        //TODO Is it possible to do it just once?
        $("div.groupItem:odd > .square").addClass('odd');
    });
}

//TODO !!!
function loadInitialData() {

    //Initial values
    viewModel.SudokuList = ko.observableArray([]);
    viewModel.CurrentSudoku = ko.observable(null);
    viewModel.CurrentHorizontalGroupList = ko.observableArray([]);

    //Get list
    $.get('/api/SudokuApi/list', function (sudokuList) {
        viewModel.SudokuList(sudokuList);

        //Get sudoku
        $.get('/api/SudokuApi/item/' + 1, function (sudoku) {

            viewModel.CurrentSudoku(sudoku);

            //Get squares
            $.get('/api/SudokuApi/squaretypegroups/' + 1, function (list) {

                viewModel.CurrentHorizontalGroupList(list);

                ko.applyBindings(viewModel);

                //Square group styling
                //TODO Is it possible to do it just once?
                $("div.groupItem:odd > .square").addClass('odd');
            });

        });

    });

}
