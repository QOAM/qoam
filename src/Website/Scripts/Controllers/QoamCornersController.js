var QoamCornersController = (function () {
    function QoamCornersController() {
    }
    QoamCornersController.prototype.index = function (journalTitlesUrl, journalIssnsUrl, journalPublishersUrl, subjectsUrl, languagesUrl) {
        createTypeahead("#Title", journalTitlesUrl);
        createTypeahead("#Issn", journalIssnsUrl);
        createTypeahead("#Publisher", journalPublishersUrl);
        //createTypeahead('input.search-language', languagesUrl);

        setupLinkFilters();
        initialLinkFilterState();

        initDisciplinesSelect();

        updateSwotMatrix('#swotFilterContainer', '#SwotMatrix');

        $("#Corner").chosen({
            search_contains: true
        });

        initSwotFilter();
    };
    return QoamCornersController;
})();