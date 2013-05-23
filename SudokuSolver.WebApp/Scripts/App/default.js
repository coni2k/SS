/* JSHint Configuration */
/*jshint jquery:true*/

(function (window, document, $, History, undefined) {
    'use strict';

    $(function () {

        // App view model
        var self = this;
        self.CurrentContentId = '';

        // History
        if (!History.enabled) {

            // TODO Is there anything we should do about this block?
            // Tell the user that the current browser is not supported or wha'.. ?!

            // History.js is disabled for this browser.
            // This is because we can optionally choose to support HTML4 browsers or not.
            return false;
        }

        // Ajax - loading message
        $(this).ajaxStart(function () {
            $('#loadingMessagePanel').dialog('open');
        }).ajaxStop(function () {
            $('#loadingMessagePanel').dialog('close');
        });

        // History - bind to StateChange Event
        History.Adapter.bind(window, 'statechange', function () { // Note: We are using statechange instead of popstate
            self.LoadContent(History);
        });

        // Links - bind to click event of an anchor
        $('.contentLink').on('click', function (event) {

            // Get the url
            var url = $(this).attr('href');

            // Push the state
            History.pushState(null, null, url);

            // Cancel the default action
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

        self.LoadContent = function () {

            // Default content
            var contentId = 'contact';

            // Get the current state
            var state = History.getState();

            // If there is a state (can be null if it's the first load or reset list)
            if (state !== null) {

                // Remove the host + default.aspx
                console.log(state.url);
                console.log(History.getRootUrl());

                var url = state.url.replace(History.getRootUrl(), '').replace('default.aspx', '');

                // If there is something to parse
                if (url !== '') {
                    contentId = url.split('/')[0];
                }
            }

            // If it's a new content, load
            if (self.CurrentContentId != contentId) {

                self.CurrentContentId = contentId;

                // Get the content from server
                getData('/Views/' + contentId + '.html', function (contentData) {
                    document.title = 'Sudoku Solver - ' + capitaliseFirstLetter(contentId);
                    $('#contentHeader').html(capitaliseFirstLetter(contentId));
                    $('#contentBody').html(contentData);
                });
            };
        };

        // Load the current content
        self.LoadContent(History);

    });

})(window, window.document, window.jQuery, window.History);

function capitaliseFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function getData(url, callback) {

    $.ajax({
        url: url,
        success: callback
    }).fail(function (jqXHR) { handleError(jqXHR); });

}

function handleError(jqXHR) {

    // Get the message
    var validationResult = $.parseJSON(jqXHR.responseText).Message;

    // Show
    // TODO ?!
    showMessagePanel(validationResult);
}
