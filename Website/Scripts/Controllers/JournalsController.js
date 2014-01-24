var JournalsController = (function () {
    function JournalsController() {
    }
    JournalsController.prototype.index = function (journalTitlesUrl, journalIssnsUrl, journalPublishersUrl) {
        $('#Title').typeahead({ prefetch: journalTitlesUrl });
        $('#Issn').typeahead({ prefetch: journalIssnsUrl });
        $('#Publisher').typeahead({ prefetch: journalPublishersUrl });

        $('#MinimumBaseScore, #MinimumValuationScore').each(function () {
            var sliderElement = $(this);

            sliderElement.slider({
                step: 0.1,
                min: parseFloat(sliderElement.attr('data-val-range-min')),
                max: parseFloat(sliderElement.attr('data-val-range-max')),
                value: parseFloat(sliderElement.val()) || 0,
                normalizeValue: function(value) {
                    return Number(value).toFixed(1);
                }
            });
        });
        
        $('ul.journal-tabs').on('shown.bs.tab', function (e) {
            var targetElement = $(e.target);
            var dataUrl = targetElement.attr('data-url');

            if (dataUrl !== undefined) {
                var href = targetElement.attr('href');
                $(href).load(dataUrl, function () { });
            }
        });

        $('a[data-select-tab="true"]').on('click', function () {
            $('a[href="' + $(this).attr('href') + '"][data-toggle="tab"]').tab('show');
            return false;
        });

        $('ul.journal-tabs').on('click', 'li.active a', function () {
            $('.journal-tabs li.active, .tab-pane.active').removeClass('active');
            return false;
        });
    };
    
    JournalsController.prototype.prices = function (viewJournalPricesElementId, viewJournalPricesModalElementId, viewInstitutionalPricesElementId, viewInstitutionalPricesModalElementId, viewJournalStandardPricesElementId, viewJournalStandardPricesModalElementId) {
        $('#' + viewJournalPricesElementId).on('click', function () {            
            $('#' + viewJournalPricesModalElementId).modal();
            return false;
        });
        
        $('#' + viewJournalStandardPricesElementId).on('click', function () {
            $('#' + viewJournalStandardPricesModalElementId).modal();
            return false;
        });

        $('#' + viewInstitutionalPricesElementId).on('click', function () {
            $('#' + viewInstitutionalPricesModalElementId).modal();
            return false;
        });
    };

    return JournalsController;
})();