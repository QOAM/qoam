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