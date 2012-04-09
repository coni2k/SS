$(function () {

    //Init model
    initModel();

    /* Event handlers */

    //Load sudoku
    $("a.load").live('click', function () {
        //Get the current id from UI
        var sudokuId = $(this).data('sudokuId');

        loadSudoku(sudokuId);
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
                    loadSudokuByData(data);
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
                    loadSudokuList();

                    //Load default (first) sudoku
                    loadSudoku(1);
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
                    viewModel.CurrentSudoku().AutoSolve = !viewModel.CurrentSudoku().AutoSolve;

                    //Load sudoku?
                    loadSudoku(viewModel.CurrentSudoku().SudokuId);

                    //Apply bindings again?
                    //ko.applyBindings(viewModel);

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
                    loadSudoku(viewModel.CurrentSudoku().SudokuId);

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
    $("input.square").live({ mouseenter: function () { $(this).addClass('selectedSquare'); }, mouseleave: function () { $(this).removeClass('selectedSquare'); } });

    //Square input change
    $("input.square").live('change', function () {

        var squareId = $(this).data('squareId');
        var number = $(this).val();

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

                    //Apply?
                    //ko.applyBindings(viewModel);

                    //TODO How to update UI, in case of AutoSolve?

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
    loadSudokuList();

    //Load default (first) sudoku
    loadSudoku(1);
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

    $.get('/api/SudokuApi/list', function (data) {
        viewModel.SudokuList(data);

        ko.applyBindings(viewModel);
    });
}

function loadSudoku(sudokuId)
{
    viewModel.CurrentSudoku = ko.observable(null);
    viewModel.CurrentHorizontalGroupList = ko.observableArray([]);

    //Get sudoku
    $.get('/api/SudokuApi/item/' + sudokuId, function (data) {
        viewModel.CurrentSudoku(data);

        ko.applyBindings(viewModel);
    });

    //Get the squares!
    //TODO Item and the squares can be retrieved together? Or remove the list from the first one!
    $.get('/api/SudokuApi/squaretypegroups/' + sudokuId, function (data) {
        viewModel.CurrentHorizontalGroupList(data);

        ko.applyBindings(viewModel);

        //Square group styling
        //TODO Is it possible to do it just once?
        $("div.groupItem:odd > .square").addClass('odd');

    });
}

//TODO This is almost the same with the above ?!
function loadSudokuByData(sudoku)
{
    viewModel.CurrentSudoku = ko.observable(null);
    viewModel.CurrentHorizontalGroupList = ko.observableArray([]);

    //Load sudoku
    viewModel.CurrentSudoku(sudoku);
    ko.applyBindings(viewModel);

    //Get the squares!
    //TODO Item and the squares can be retrieved together? Or remove the list from the first one!
    $.get('/api/SudokuApi/squaretypegroups/' + sudoku.SudokuId, function (list) {
        viewModel.CurrentHorizontalGroupList(list);

        ko.applyBindings(viewModel);

        //Square group styling
        //TODO Is it possible to do it just once?
        $("div.groupItem:odd > .square").addClass('odd');


        //$("div.groupItem:odd > .square").addClass('odd');


    });
}

function determineClassByAssignType(type)
{
    alert('type: ' + type);
}