/* JSHint Configuration */
/*jshint jquery:true*/
/*global ko:true, Enumerable:true*/

(function (window, document, $, Enumerable, History, undefined) {
    'use strict';

    // + Variables
    var
        /* WebAPI URLs */
        /* Root */
        apiUrlContentRoot = '/api/Content/',
        apiUrlGetContent = function (contentInternalId) { return apiUrlContentRoot + contentInternalId; };

    $(function () {

        // App view model
        var self = this;
        self.Contents = [];
        self.CurrentContent = null;

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

        // History - Bind StateChange Event
        History.Adapter.bind(window, 'statechange', function () { // Note: We are using statechange instead of popstate
            self.LoadContent();
        });

        // Loading message modal
        $("#loadingMessagePanel").dialog({
            dialogClass: 'ui-dialog-notitlebar',
            resizable: false,
            height: 70,
            width: 250,
            modal: true
        });

        // Load list
        self.LoadContents = function () {

            // Not async.
            getData(apiUrlContentRoot, function (contents) {
                
                self.Contents = contents;

                var contentLinksHtml = [];
                Enumerable.From(self.Contents).ForEach(function (content) {

                    var linkHtml = '';

                    if (content.IsExternal) {
                        linkHtml = $('#externalLinkTemplate').html().trim();
                    } else {
                        linkHtml = $('#internalLinkTemplate').html().trim();
                    };

                    linkHtml = linkHtml.replace('[Url]', content.Url).replace('[Title]', content.Title);

                    contentLinksHtml.push(linkHtml);
                });

                $('#contentLinks').html(contentLinksHtml.join(''));

                // Links - Bind anchor click event
                $('.internalLink').on('click', function (event) {

                    // Get the url
                    var url = $(this).attr('href');

                    // Push the state
                    History.pushState(null, null, url);

                    // Cancel the default action
                    event.preventDefault();
                    return false;

                });

            }, false);
        };

        self.LoadContent = function () {

            // Default content
            var contentId = 'contact';

            // Get the current state
            var state = History.getState();

            // If there is a state (can be null if it's the first load or reset list)
            if (state !== null) {

                // Remove the host + default.aspx
                var url = state.url.replace(History.getRootUrl(), '').replace('default.aspx', '');

                // If there is something to parse
                if (url !== '') {
                    contentId = url.split('/')[0];
                }
            };

            // Search for the item
            var currentContent = Enumerable.From(self.Contents).SingleOrDefault(null, function (content) {
                return content.InternalId === contentId;
            });

            // There is no content with the current id, get the first one as the default
            // Navigate to 404 ?
            if (currentContent === null) {
                return;
            };

            // If it's a new content, load
            if (self.CurrentContent !== currentContent) {

                self.CurrentContent = currentContent;

                $('#contentContainer').fadeOut('fast', function () {

                    document.title = 'Sudoku Solver - ' + currentContent.Title;
                    $('#contentHeader').html(currentContent.Title);
                    $('#contentBody').html(currentContent.Body);

                    $('#contentContainer').fadeIn('fast');
                });                
            };
        };

        // Load the current content
        self.LoadContents(); // Async false
        self.LoadContent();

    });

})(window, window.document, window.jQuery, window.Enumerable, window.History);

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

function handleError(jqXHR) {

    // Get the message
    var validationResult = $.parseJSON(jqXHR.responseText).Message;

    // Show
    // TODO ?!
    showMessagePanel(validationResult);
}
