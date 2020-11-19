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
            var $this = $(this);
            e.preventDefault();
            $("#submit-trust-modal-title").html($this.data("title"));
            $("#submit-trust").data("journalid", $this.data("journalid"));
            $("#submit-trust-modal").modal();

        });

        $("#trust-checkbox").change(function() {
            $("#submit-trust").prop("disabled", !$(this).prop("checked"));
        });

        $("#submit-trust").click(function(e) {
            e.preventDefault();

            var $this = $(this);
            $this.prop("disabled", true);
            $("#submitting-trust-loader").show();

            var url = $this.data("url");
            $.post(url, {
                journalId: $this.data("journalid"),
                institutionId: $this.data("institutionid"),
                userId: $this.data("userid")
            }, function(data) {
                $("#submitting-trust-loader").hide();
                $(".success-status").show();
                
                $("#submitting-trust-loader").hide();
                $this.hide();
                $(".default-status").hide();
            });
        });


        $("#submit-trust-modal").on("hidden.bs.modal", function() {
            $("#submit-trust").prop("disabled", false);
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