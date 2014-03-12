var InstitutionsController = (function () {
    function InstitutionsController() {
    }
    InstitutionsController.prototype.index = function (institutionNamesUrl) {
        $('#Name').typeahead({ remote: institutionNamesUrl + '?query=%QUERY' });
    };
    return InstitutionsController;
})();