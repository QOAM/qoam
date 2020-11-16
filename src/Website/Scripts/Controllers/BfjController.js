var BfjController = (function () {
    function BfjController() {
    }

    BfjController.prototype.index = function (journalTitlesUrl, journalIssnsUrl, journalPublishersUrl, subjectsUrl, languagesUrl) {
        createTypeahead('#Title', journalTitlesUrl);
        createTypeahead('#Issn', journalIssnsUrl);
        createTypeahead('#Publisher', journalPublishersUrl);
        createTypeahead('input.search-language', languagesUrl);

        initDisciplinesSelect();

        $(".grey").click(function (e) {
            e.preventDefault();
            $("#submit-trust-modal-title").html($(this).data("title"));
            $("#submit-trust-modal").modal();

        });

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

        
        $('a.help-window').on('click', function() {
            window.open($(this).attr('href'), 'Help', 'width=800,height=600,toolbar=no');
            return false;
        });
    };

    return BfjController;
})();