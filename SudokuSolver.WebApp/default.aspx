<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Sudoku Solver</title>
    <link href="/Content/main.css?v=20130515.1" rel="stylesheet" type="text/css" />
    <link href="/Content/themes/base/minified/jquery-ui.min.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.9.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="/Scripts/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="/Scripts/knockout.mapping-latest.js" type="text/javascript"></script>
    <script src="/Scripts/linq.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.history-1.7.1.min.js"></script>
</head>
<body>
    <div id="mainContainer">
        <div id="mainHeaderPanel" class="panel">
            <div class="contentLeft">&nbsp;</div>
            <div class="contentRight">
                <h1>Sudoku Killa</h1>
            </div>
        </div>
        <div id="menuPanel" class="panel">
            <div class="contentLeft">&nbsp;</div>
            <div class="contentRight">
                <div id="contentLinks" />
            </div>
        </div>
        <div id="contentContainer">
            <div id="contentHeaderPanel" class="panel">
                <div class="contentLeft">&nbsp;</div>
                <div class="contentRight">
                    <h2 id="contentHeader" />
                </div>
            </div>
            <div id="contentBodyPanel" class="panel">
                <div id="contentBody" />
            </div>
        </div>
    </div>
    <div id="loadingMessagePanel" class="hide">
        <span id="loadingMessageText">Loading, please wait!</span>
        <img id="loadingMessageImage" src="/Images/ajax-loader.gif" />
    </div>

    <!-- Html Templates -->
    <script id="internalLinkTemplate" type="text/html">
        <a href="[Url]" class="internalLink">[Title]</a>
    </script>
    <script id="externalLinkTemplate" type="text/html">
        <a href="[Url]">[Title]</a>
    </script>
    <!-- Html templates end -->

    <script src="/Scripts/App/default.js" type="text/javascript"></script>
</body>
</html>
