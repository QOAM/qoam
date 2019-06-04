var QoamCornersController = (function () {
    function QoamCornersController() {
    }
    QoamCornersController.prototype.index = function (journalTitlesUrl, journalIssnsUrl, journalPublishersUrl, subjectsUrl, languagesUrl) {
        window.createTypeahead("#Title", journalTitlesUrl);
        window.createTypeahead("#Issn", journalIssnsUrl);
        window.createTypeahead("#Publisher", journalPublishersUrl);
        //createTypeahead('input.search-language', languagesUrl);

        window.setupLinkFilters();
        window.initialLinkFilterState();

        window.initDisciplinesSelect();

        //updateSwotMatrix('#swotFilterContainer', '#SwotMatrix');

        $("#Corner").chosen({
            search_contains: true
        });

        //initSwotFilter();

        $("#discard-corner").click(function (e) {
            var deleteUrl = this.href;
            e.preventDefault();
            $("#discard-corner-modal").modal();

            $("#confirm-discard").on("click", function () {
                window.location.href = deleteUrl;
            });
        });
    };
    return QoamCornersController;
})();