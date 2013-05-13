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
        ko = window.ko,
        Enumerable = window.Enumerable;

    // Bind knockout
    var xModel = new XModel();
    ko.applyBindings(xModel);

    // + Objects
    function XModel() {

        var self = this;

        self.Types = ko.observableArray([]);
        self.Types.push('1');
        self.Types.push('2');
        self.Types.push('3');
        self.Types.push('4');

    }

})(window);
