var InstitutionsController = (function () {
    function InstitutionsController() {
    }
    InstitutionsController.prototype.index = function (institutionNamesUrl) {
        createTypeahead('#Name', institutionNamesUrl);
    };
    return InstitutionsController;
})();