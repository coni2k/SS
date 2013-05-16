/* JSHint Configuration */
/*jshint jquery:true*/
/*global ko:true, Enumerable:true*/

(function (window, undefined) {
    'use strict';

    // + Variables
    var
        /* Globals */
        document = window.document,
        $ = window.jQuery,
        history = window.History;

    $(function () {

        // Setup

        //// a. jQuery ajax
        //$.ajaxSetup({
        //    contentType: 'application/json; charset=utf-8'
        //});

        // b. History
        if (!history.enabled) {

            // TODO Is there anything we should do about this block?
            // Tell the user that the current browser is not supported or wha'.. ?!

            // History.js is disabled for this browser.
            // This is because we can optionally choose to support HTML4 browsers or not.
            return false;
        }

        // Ajax - loading message
        $(this).ajaxStart(function () {

            // Hide previous error message
            // hideMessagePanel();

            $('#loadingMessagePanel').dialog('open');

        }).ajaxStop(function () {
            $('#loadingMessagePanel').dialog('close');
        });

        // History - bind to StateChange Event
        history.Adapter.bind(window, 'statechange', function () { // Note: We are using statechange instead of popstate
            loadContent(history);
        });

        // Links - Bind to click event of an anchor?
        $('.contentLink').on('click', function (event) {

            // Get the url
            var url = $(this).attr('href');

            // Push the state
            history.pushState(null, null, url);

            // Cancel the event
            event.preventDefault();
            return false;

        });

        // Loading message modal
        $("#loadingMessagePanel").dialog({
            dialogClass: 'ui-dialog-notitlebar',
            resizable: false,
            height: 70,
            width: 250,
            modal: true
        });

        //// Message panel - clear
        //$('#messagePanelClear').live('click', function () {
        //    hideMessagePanel();
        //});

        // Load the current content
        loadContent(history);

    });

})(window);

function loadContent(history) {

    //// Hide previous messages
    // hideMessagePanel();

    // Variables
    var contentId = 'contact';

    var state = history.getState();

    // If there is a state (can be null if it's the first load or reset list)
    if (state !== null) {

        // Remove the host part
        var url = state.url.replace(history.getRootUrl(), '').replace('default.aspx', '');

        // If there is something to parse
        if (url !== '') {

            if (url.indexOf('/') === 0) { // There is no Id, use the url itself (for Contact, License, Source pages)
                contentId = url;
            }
            else { // This is one of the sudokus, get the Id as well
                contentId = url.split('/')[0];
            }
        }
    }

    // Get the content from server
    getData('/Views/' + contentId + '.html', function (content) {
        document.title = 'Sudoku Solver - ' + contentId;
        $('#contentHeader').html(contentId);
        $('#contentRight').html(content);
    });
}

//function showMessagePanel(message) {
//    $('#messagePanelMessage').text(message);
//    $('#messagePanel').fadeTo(200, 1);
//}

//function hideMessagePanel() {
//    $('#messagePanel').fadeTo(200, 0.01);
//}

function getData(url, callback) {

    $.ajax({
        async: true,
        url: url,
        success: callback
    }).fail(function (jqXHR) { handleError(jqXHR); });

}

// Error handling
function handleError(jqXHR) {

    // Get the message
    var validationResult = $.parseJSON(jqXHR.responseText).Message;

    // Show
    showMessagePanel(validationResult);
}