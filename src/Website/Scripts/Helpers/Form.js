function initializeForm(culture) {

    Globalize.culture(culture);

    $('span.field-validation-valid, span.field-validation-error').each(function () {
        $(this).addClass('help-inline');
    });

    $('.validation-summary-errors').each(function () {
        $(this).addClass('alert alert-error');
    });

    $('form').submit(function () {
        if ($(this).valid()) {
            $(this).find('div.form-group').each(function () {
                if ($(this).find('span.field-validation-error').length == 0) {
                    $(this).removeClass('has-error');
                }
            });
        }
        else {
            $(this).find('div.form-group').each(function () {
                if ($(this).find('span.field-validation-error').length > 0) {
                    $(this).addClass('has-error');
                }
            });
        }
    });

    $('form').each(function () {
        $(this).find('div.form-group').each(function () {
            if ($(this).find('span.field-validation-error').length > 0) {
                $(this).addClass('has-error');
            }
        });
    });
}

function createTypeahead(selector, remoteUrl) {
    var bloodhoundInstance = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: remoteUrl + '?query=%QUERY'
    });

    bloodhoundInstance.initialize();

    $(selector).typeahead(null, {
        displayKey: 'value',
        source: bloodhoundInstance.ttAdapter()
    });
}

function updateSwotMatrix(selector, input) {
    $($(input).val().split(',')).each(function () {
        if (this.length > 0) {
            $(selector).find('.' + this).addClass('verdict-' + this).addClass('selected');
        }
    });
}

function setupLinkFilters() {
    $("#open-access-filter").on("click", function (e) {
        e.preventDefault();

        var currentValue = $("#OpenAccess").val();

        if (currentValue === "" || currentValue === false || currentValue.toLowerCase() === "false")
            $("#OpenAccess").val(true);
        else
            $("#OpenAccess").val("");

        $(this).toggleClass("filter-enabled");
        $("#hybrid-filter").removeClass("filter-enabled");
    });

    $("#hybrid-filter").on("click", function (e) {
        e.preventDefault();

        var currentValue = $("#OpenAccess").val();

        if (currentValue === "" || currentValue === true || currentValue.toLowerCase() === "true")
            $("#OpenAccess").val(false);
        else
            $("#OpenAccess").val("");

        $(this).toggleClass("filter-enabled");
        $("#open-access-filter").removeClass("filter-enabled");
    });
}