$(function () {

    // We're using a Knockout model. This clears out the existing comments.
    viewModel.SudokuList([]);
    viewModel.CurrentSudoku(null);
    viewModel.CurrentHorizontalGroupList([]);

    $.get('/api/SudokuApi/list', function (data) {
        viewModel.SudokuList(data);
    });

    //Load sudoku event
    $("a.load").live('click', function () {
        //Get the current id from UI
        var sudokuId = $(this).data('sudokuId');

        loadSudoku(sudokuId);
    });

    //Square input focus + hover events
    $("input.square").live('focus', function () { $(this).select(); } );
    $("input.square").live({ mouseenter: function () { $(this).addClass('selected'); }, mouseleave: function () { $(this).removeClass('selected'); } });

    //Square input change event
    $("input.square").live('change', function () {

        var squareId = $(this).data('squareId');
        var number = $(this).val();

        var squareContainer = { SquareId: squareId, Number: number };
        var json = JSON.stringify(squareContainer);

        $.ajax({
            url: '/api/SudokuApi/fillsquare/' + viewModel.CurrentSudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            data: json,
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*Created*/: function (data) {

                    //Updat UI ??!

                    //Apply?
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

    //Toggle auto solve event
    $("a.toggleAutoSolve").live('click', function () {

        $.ajax({
            url: '/api/SudokuApi/toggleautosolve/' + viewModel.CurrentSudoku().SudokuId,
            cache: false, //?
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            statusCode: {
                200 /*Created*/: function (data) {

                    //Updat UI
                    viewModel.CurrentSudoku().AutoSolve = !viewModel.CurrentSudoku().AutoSolve;

                    //Apply?
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

    //Load default (first) sudoku
    loadSudoku(1);

    //Bindings
    ko.applyBindings(viewModel);
});

function loadSudoku(sudokuId)
{
    viewModel.CurrentSudoku = ko.observable(null);
    viewModel.CurrentHorizontalGroupList = ko.observableArray([]);

    $.get('/api/SudokuApi/item/' + sudokuId, function (data) {
        viewModel.CurrentSudoku(data);

        ko.applyBindings(viewModel);
    });

    $.get('/api/SudokuApi/horizontaltypegroups/' + sudokuId, function (data) {
        viewModel.CurrentHorizontalGroupList(data);

        ko.applyBindings(viewModel);
    });
}
