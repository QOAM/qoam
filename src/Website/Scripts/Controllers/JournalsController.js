var JournalsController = (function () {
    function JournalsController() {
    }

    JournalsController.prototype.index = function (journalTitlesUrl, journalIssnsUrl, journalPublishersUrl, subjectsUrl, languagesUrl) {
        createTypeahead('#Title', journalTitlesUrl);
        createTypeahead('#Issn', journalIssnsUrl);
        createTypeahead('#Publisher', journalPublishersUrl);
        //createTypeahead('input.search-discipline', subjectsUrl);
        createTypeahead('input.search-language', languagesUrl);

        setupLinkFilters();
        initialLinkFilterState();

        initDisciplinesSelect();

        updateSwotMatrix('#swotFilterContainer', '#SwotMatrix');

        $('.remove-discipline').on('click', function(e) {
            $(this).closest('li').remove();

            $('#disciplines input').each(function (index, element) {
                $(element).attr('name', 'Disciplines[' + index + ']');
            });

            e.preventDefault();
            $('#search-form').submit();

            return false;
        });

        $('.remove-language').on('click', function (e) {
            $(this).closest('li').remove();

            $('#languages input').each(function (index, element) {
                $(element).attr('name', 'Languages[' + index + ']');
            });

            e.preventDefault();
            $('#search-form').submit();

            return false;
        });

        initSwotFilter();

        //$('#swotFilterContainer').on('click', 'div.table-cell', function() {
        //    $(this).toggleClass('verdict-' + $(this).attr('data-swot-type')).toggleClass('selected');
        //    return false;
        //});

        //$('#submitForm').on('click', function () {
        //    var swotInput = $('#SwotMatrix');
        //    swotInput.val('');
        //    $('#swotFilterContainer').find('div.selected').each(function () {
        //        var currentValue = swotInput.val();
        //        var swotType = $(this).attr('data-swot-type');

        //        if (currentValue === '') {
        //            swotInput.val(swotType);
        //        }
        //        else {
        //            swotInput.val(currentValue + ',' + swotType);
        //        }
        //    });
        //});

        $('a.help-window').on('click', function() {
            window.open($(this).attr('href'), 'Help', 'width=800,height=600,toolbar=no');
            return false;
        });
    };

    JournalsController.prototype.details = function () {

        $('ul.journal-tabs').on('shown.bs.tab', function (e) {
            var targetElement = $(e.target);
            var dataUrl = targetElement.attr('data-url');

            if (dataUrl !== undefined) {
                var href = targetElement.attr('href');
                $(href).load(dataUrl, function() {});
            }
        });

        $('a[data-select-tab="true"]').on('click', function() {
            $('a[href="' + $(this).attr('href') + '"][data-toggle="tab"]').tab('show');
            return false;
        });

        $('ul.journal-tabs').on('click', 'li.active a', function() {
            $(this).closest('.tabbablesearch').find('.journal-tabs li.active, .tab-pane.active').removeClass('active');
            return false;
        });
    };

    JournalsController.prototype.prices = function (viewJournalPricesElementId, viewJournalPricesModalElementId, viewJournalStandardPricesElementId, viewJournalStandardPricesModalElementId) {
        var table = $("#licenses").DataTable({
            "ordering": false,
            "info": false,
            "lengthChange": false,
            "dom": "tp",
            "pagingType": "numbers"
        });

        $('#FilterInstitution').on('keyup', function () {
            table.search(this.value).draw();
        });

        if ($('#FilterInstitution').length === 0) {
            $('#licenses_paginate').hide();
        }

        $('#' + viewJournalPricesElementId).on('click', function () {
            $('#' + viewJournalPricesModalElementId).modal();
            return false;
        });
        
        $('#' + viewJournalStandardPricesElementId).on('click', function () {
            $('#' + viewJournalStandardPricesModalElementId).modal();
            return false;
        });
    };

    return JournalsController;
})();