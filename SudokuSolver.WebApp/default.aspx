<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Sudoku Solver</title>
    <link href="/Content/sudokusolver.css" rel="stylesheet" type="text/css" />
    <link href="/Content/themes/base/minified/jquery-ui.min.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.9.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="/Scripts/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="/Scripts/knockout.mapping-latest.js" type="text/javascript"></script>
    <script src="/Scripts/linq.min.js" type="text/javascript"></script>
    <script src="/Scripts/history/scripts/bundled/html4html5/jquery.history.js"></script>
</head>
<body>
    <div class="header">
        <h1>Sudoku Solver</h1>
    </div>
    <div class="contentContainer">
        <div class="contentLeft" data-bind="css: { hide: !HasSudokuList() }">
            <div id="sudokuListPanel" class="panel">
                <strong>Sudoku List</strong>
                <!--ko foreach: SudokuList-->
                <div class="sudokuItem">
                    <!--<a data-bind="click: $parent.SelectSudoku, attr: { href: '/sudoku/?id=' + SudokuId() }">-->
                    <a data-bind="click: $parent.Navigate, attr: { href: '/sudoku/?id=' + SudokuId }">
                        <!--ko text: 'Id: ' + SudokuId + ' - Title: ' + Title-->
                        <!--/ko-->
                    </a>
                </div>
                <!--/ko-->
                <a data-bind="click: NewSudoku" href="#">New sudoku</a>
                <a data-bind="click: ResetList" href="#">Reset list</a><br />
                <a href="/help">Help</a>
                <a href="/contact">Contact</a><br />
                <a href="/source">Source</a>
                <a href="/license">License</a>
            </div>
        </div>
        <div class="contentRight">
            <div id="header">
                <h2>
                    <!--ko text: CurrentContent-->
                    <!--/ko-->
                </h2>
            </div>
            <div id="contentPanel" data-bind="css: { hide: !HasSudoku() }">
                <div id="detailsPanel" class="panel">
                    <strong>Details</strong>
                    <br />
                    Sudoku Id:
                    <!--ko text: Sudoku().SudokuId-->
                    <!--/ko-->
                    <br />
                    Squares left:
                    <!--ko text: Sudoku().SquaresLeft-->
                    <!--/ko-->
                    <br />
                    Ready: <a id="toggleReady" data-bind="click: Sudoku().ToggleReady" href="#">
                        <!--ko text: Sudoku().ReadyFormatted-->
                        <!--/ko-->
                    </a>
                    |
                <span id="solvePanel" data-bind="visible: Sudoku().Ready">Autosolve:
                    <a id="toggleAutoSolve" data-bind="click: Sudoku().ToggleAutoSolve" href="#">
                        <!--ko text: Sudoku().AutoSolveFormatted-->
                        <!--/ko-->
                    </a>
                    <span class="solve" data-bind="visible: !Sudoku().AutoSolve && Sudoku().Hints().length > 0">
                        - 
                        <a data-bind="click: Sudoku().Solve" href="#">Solve now</a>
                    </span>
                </span>
                    <span data-bind="visible: Sudoku().Resettable">- 
                    <a data-bind="click: Sudoku().Reset" href="#">Reset</a>
                    </span>
                    <br />
                    <span>Display: Values | Availabilities: 
                    <a data-bind="click: Sudoku().ToggleDisplayAvailabilities" href="#">
                        <!--ko text: Sudoku().DisplayAvailabilitiesFormatted-->
                        <!--/ko-->
                    </a><span>|
                    </span>
                        <span>IDs: </span>
                        <a data-bind="click: Sudoku().ToggleDisplayIDs" href="#">
                            <!--ko text: Sudoku().DisplayIDsFormatted-->
                            <!--/ko-->
                        </a>
                    </span>
                </div>
                <div id="selectedSquarePanel" class="panel">
                    <strong>Selected square</strong>
                    <br />
                    <span>Square Id: </span>
                    <!--ko with: Sudoku().SelectedSquare-->
                    <!--ko text: SquareId-->
                    <!--/ko-->
                    <!--/ko-->
                    <br />
                    <span>Value: </span>
                    <!--ko with: Sudoku().SelectedSquare-->
                    <span data-bind="if: !IsAvailable()">
                        <span data-bind="html: ValueFormatted"></span>
                        <!--ko if: $root.Sudoku().IsSelectedSquareValueRemoveable-->
                        <a data-bind="click: $root.Sudoku().RemoveSelectedSquareValue" href="#">[x]</a>
                        <!--/ko-->
                    </span>
                    <!--/ko-->
                </div>
                <div id="selectedNumberPanel" class="panel">
                    <strong>Selected number</strong>
                    <br />
                    <span>Number: </span>
                    <!--ko with: Sudoku().SelectedNumber-->
                    <!--ko text: Value-->
                    <!--/ko-->
                    <!--/ko-->
                    <br />
                    <span>Count: </span>
                    <!--ko with: Sudoku().SelectedNumber-->
                    <!--ko text: Count-->
                    <!--/ko-->
                    <!--/ko-->
                </div>
                <div id="legendPanel" class="panel hide">
                    <strong>Legend</strong>
                    <br />
                    <span class="initial">Initial</span>
                    <br />
                    <span class="user">User</span>
                    <br />
                    <span class="solver">Solver</span>
                </div>
                <div id="messagePanel" class="panel almostHide">
                    <span class="error">
                        <span id="messagePanelMessage"></span><a id="messagePanelClear" href="#">[x]</a>
                    </span>
                </div>
                <div id="numberGridPanel" data-bind="foreach: Sudoku().NumberGroups, css: Sudoku().CssValueGrid">
                    <div data-bind="foreach: Numbers, css: CssClass">
                        <div data-bind="click: $root.Sudoku().SetSelectedNumber, attr: { 'class': CssClass }, css: { activeSelected: IsActiveSelected, passiveSelected: IsPassiveSelected(), odd: $parent.IsOdd }">
                            <span class="value" data-bind="text: Value"></span>
                        </div>
                    </div>
                </div>
                <div id="valueGridPanel" data-bind="template: { name: 'gridTemplate', data: Sudoku().ValueGrid }, css: Sudoku().CssValueGrid">
                </div>
                <div id="availabilityGridPanel" data-bind="template: { name: 'gridTemplate', data: Sudoku().AvailabilityGrid }, css: Sudoku().CssAvailabilityGrid">
                </div>
                <div id="idGridPanel" data-bind="template: { name: 'gridTemplate', data: Sudoku().IdGrid }, css: Sudoku().CssIdGrid">
                </div>
                <div id="hintsPanel" class="panel">
                    <strong>Hints</strong>
                    <br />
                    <!--ko foreach: Sudoku().Hints-->
                    <div class="hintItem" data-bind="event: { mouseenter: ToggleSelect, mouseleave: ToggleSelect }, css: { passiveSelected: IsSelected() }">
                        <!--ko text: 'SquareId: ' + SquareId + ' - HintValue: ' + HintValue + ' - HintType: ' + HintType-->
                        <!--/ko-->
                    </div>
                    <!--/ko-->
                </div>
                <div id="groupNumberAvailabilityPanel" class="panel">
                    <strong>Group Number Availability</strong>
                    <br />
                    <!--ko foreach: Sudoku().GroupNumberAvailabilities-->
                    <div class="" data-bind="">
                        <!--ko text: 'GroupId: ' + GroupId + ' - Number: ' + Number + ' - Count: ' + Count-->
                        <!--/ko-->
                    </div>
                    <!--/ko-->
                </div>
            </div>
        </div>
    </div>
    <!-- Html templates -->
    <!-- Loading message -->
    <div id="loadingMessagePanel" class="hide">
        <span id="loadingMessageText">Loading, please wait!</span>
        <img id="loadingMessageImage" src="/Images/ajax-loader.gif" />
    </div>
    <!-- New sudoku dialog -->
    <div id="newSudokuDialog" title="New sudoku" class="hide">
        <form id="newSudokuForm">
            <table>
                <tr>
                    <td class="label">
                        <label for="newSudokuSize">
                            Size
                        </label>
                    </td>
                    <td>
                        <select id="newSudokuSize" name="newSudokuSize" class="required">
                            <option value="4">4</option>
                            <option value="9" selected="selected">9</option>
                            <option value="16">16</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <label for="newSudokuTitle">
                            Title
                        </label>
                    </td>
                    <td>
                        <input id="newSudokuTitle" name="newSudokuTitle" type="text" class="required" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <label for="newSudokuDescription">
                            Description
                        </label>
                    </td>
                    <td>
                        <textarea id="newSudokuDescription" name="newSudokuDescription"></textarea>
                    </td>
                </tr>
            </table>
        </form>
    </div>
    <!-- Reset dialog -->
    <!--<script id="resetDialog-template" type="text/html">-->
    <div id="resetDialog" title="Reset sudoku?" class="hide">
        <p>
            Are you sure you want to reset the sudoku?
        </p>
    </div>
    <!--</script>-->
    <!-- Reset list dialog -->
    <!--<script id="resetListDialog-template" type="text/html">-->
    <div id="resetListDialog" title="Reset list?" class="hide">
        <p>
            Are you sure you want to reset the list?
        </p>
    </div>
    <!--</script>-->
    <!-- Sudoku grid template -->
    <script id="gridTemplate" type="text/html">
        <!--ko foreach: Groups-->
        <div class="groupItem" data-bind="foreach: Squares, css: CssClass">
            <div data-bind="template: { name: $parents[1].Template, data: $data }, click: $root.Sudoku().SetSelectedSquare, attr: { 'class': CssClass }, css: { initial: AssignType() === 0, user: AssignType() === 1, hint: AssignType() === 2, solver: AssignType() === 3, activeSelected: IsActiveSelected, passiveSelected: IsPassiveSelected(), relatedSelected: IsRelatedSelected(), odd: $parent.IsOdd }">
            </div>
        </div>
        <!--/ko-->
    </script>
    <script id="squareValueTemplate" type="text/html">
        <span data-bind="html: ValueFormatted" class="value" />
    </script>
    <script id="squareAvailabilitiesTemplate" type="text/html">
        <div data-bind="foreach: Availabilities">
            <span class="availabilityItem" data-bind="text: Value, css: { unavailable_self: !$parent.IsAvailable(), unavailable_group: !IsAvailable() }" />
        </div>
    </script>
    <script id="squareIdTemplate" type="text/html">
        <span class="value" data-bind="text: SquareId" />
    </script>
    <!-- Html templates End -->
    <script src="/Scripts/sudokusolver.js" type="text/javascript"></script>
</body>
</html>
