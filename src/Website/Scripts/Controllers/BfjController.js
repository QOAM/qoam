var BfjController = (function () {
    function BfjController() {
    }

    BfjController.prototype.index = function (journalTitlesUrl, journalIssnsUrl, journalPublishersUrl, subjectsUrl, languagesUrl) {
        createTypeahead('#Title', journalTitlesUrl);
        createTypeahead('#Issn', journalIssnsUrl);
        createTypeahead('#Publisher', journalPublishersUrl);
        createTypeahead('input.search-language', languagesUrl);

        setupLinkFilters();
        initialLinkFilterState();
        initDisciplinesSelect();

        $(".grey").not(".filter").click(function (e) {
            var $this = $(this);
            e.preventDefault();
            $(".submit-trust-journal-title").html($this.data("title"));
            $("#submit-trust").data("journalid", $this.data("journalid"));
            $("#submit-trust-modal").modal();

        });

        initSubtmitTrustDialog();

        $('.remove-discipline').on('click', function (e) {
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


        $('a.help-window').on('click', function () {
            window.open($(this).attr('href'), 'Help', 'width=800,height=600,toolbar=no');
            return false;
        });
    };

    BfjController.prototype.details = function (viewJournalPricesElementId, viewJournalPricesModalElementId, viewJournalStandardPricesElementId, viewJournalStandardPricesModalElementId) {

        $("#open-submit-trust-dialog").click(function (e) {
            var $this = $(this);
            e.preventDefault();
            $(".submit-trust-journal-title").html($this.data("title"));
            $("#submit-trust").data("journalid", $this.data("journalid"));
            $("#submit-trust-modal").modal();

        });

        $(".remove-trust").click(function(e) {
            var $this = $(this);
            e.preventDefault();

            $("#trusting-institution").html($this.data("institution"));
            $("#remove-trust").data("url", this.href);
            $("#remove-trust-modal").modal();
        });

        $("#remove-trust").click(function(e) {
            window.location.href = $(this).data("url");
        });

        $("#back-button").click(function(e) {
            e.preventDefault();
            window.history.go(-1);
        });

        initSubtmitTrustDialog();

        var table = $("#licenses").DataTable({
            "ordering": false,
            "info": false,
            "lengthChange": false,
            "dom": "tp",
            "pagingType": "numbers"
        });

        //$('#FilterInstitution').on('keyup', function () {
        //    table.search(this.value).draw();
        //});

        //if ($('#FilterInstitution').length === 0) {
        //    $('#licenses_paginate').hide();
        //}

        $('#' + viewJournalPricesElementId).on('click', function () {
            $('#' + viewJournalPricesModalElementId).modal();
            return false;
        });
        
        $('#' + viewJournalStandardPricesElementId).on('click', function () {
            $('#' + viewJournalStandardPricesModalElementId).modal();
            return false;
        });
    };

    function initSubtmitTrustDialog() {
        $("#trust-checkbox").change(function () {
            $("#submit-trust").prop("disabled", !$(this).prop("checked"));
        });

        $("#submit-trust").click(function (e) {
            e.preventDefault();

            var $this = $(this);
            $this.prop("disabled", true);
            $("#submitting-trust-loader").show();

            var url = $this.data("url");
            var journalId = $this.data("journalid");
            $.post(url, {
                journalId: journalId,
                institutionId: $this.data("institutionid"),
                userId: $this.data("userid")
            }).done(function (data) {

                var $journalLink = $(`#${journalId}`);
                //remove the click handler, as the journal is not "grey" now.
                $journalLink.off("click");

                $("#submitting-trust-loader").hide();
                $(".success-status").show();
                $this.hide();
                $(".default-status").hide();

                var newCssSlass = "";
                switch (data.trustedByCount) {
                    case 0:
                        newCssSlass = "grey";
                        break;
                    case 1:
                    case 2:
                        newCssSlass = "lightblue";
                        break;
                    default:
                        break;
                }

                $journalLink.removeClass("grey");
                $journalLink.addClass(newCssSlass);

            }).fail(function (error) {
                if (error.status === 409) {
                    $(".conflict-status").show();
                    $("#submitting-trust-loader").hide();
                    $this.hide();
                    $(".default-status").hide();
                }
            });
        });

        $("#close-subtmit-modal").click(function() {
            window.location.reload();
        });


        //$("#submit-trust-modal").on("hidden.bs.modal", function () {
        //    $("#submit-trust").prop("disabled", false);
        //    $(".success-status, .conflict-status").hide();

        //    $("#submitting-trust-loader").hide();
        //    $("#submit-trust").show();
        //    $(".default-status").show();
        //    $("#trust-checkbox").prop("checked", false);
        //});
    }

    return BfjController;
})();