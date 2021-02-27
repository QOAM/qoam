function initializeForm(culture) {

    loadAndSetLocales(culture);

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

function loadAndSetLocales(culture) {
    $.when(
        //$.get("cldr/main/en/ca-gregorian.json"),
        $.get("/Scripts/cldr/supplemental/likelySubtags.json"),
        $.get("/Scripts/cldr/supplemental/timeData.json"),
        $.get("/Scripts/cldr/supplemental/weekData.json")
    ).then(function() {

        // Normalize $.get results, we only need the JSON, not the request statuses.
        return [].slice.apply(arguments, [0]).map(function(result) {
            return result[0];
        });

    }).then(Globalize.load).then(function() {
        Globalize.locale(culture);
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

//function updateSwotMatrix(selector, input) {
//    $($(input).val().split(',')).each(function () {
//        if (this.length > 0) {
//            $(selector).find('.' + this).addClass('verdict-' + this).addClass('selected');
//        }
//    });
//}

function setupLinkFilters() {
    $("#open-access-filter").on("click", function (e) {
        e.preventDefault();
        var $field = $("#OpenAccess");
        var currentValue = $field.val();

        if (currentValue === "" || currentValue === false || currentValue.toLowerCase() === "false")
            $field.val(true);
        else
            $field.val("");

        $(this).toggleClass("filter-enabled");
        $("#hybrid-filter").removeClass("filter-enabled");

        $("#search-form").submit();
    });

    $("#hybrid-filter").on("click", function (e) {
        e.preventDefault();
        var $field = $("#OpenAccess");
        var currentValue = $field.val();

        if (currentValue === "" || currentValue === true || currentValue.toLowerCase() === "true")
            $field.val(false);
        else
            $field.val("");

        $(this).toggleClass("filter-enabled");
        $("#open-access-filter").removeClass("filter-enabled");

        $("#search-form").submit();
    });

    toggleFilter("#institutional-discount-filter", "#InstitutionalDiscounts");
    toggleFilter("#no-fee-filter", "#NoFee");
    toggleFilter("#plan-s-filter", "#PlanS");
    toggleFilter("#blue-journals", "#Blue", "#lightblue-journals, #grey-journals", "#Lightblue, #Grey");
    toggleFilter("#lightblue-journals", "#Lightblue", "#blue-journals, #grey-journals", "#Blue, #Grey");
    toggleFilter("#grey-journals", "#Grey", "#blue-journals, #lightblue-journals", "#Blue, #Lightblue");
}

function initialLinkFilterState() {
    var openAccess = $("#OpenAccess").val();

    if (openAccess !== undefined && openAccess !== null && openAccess !== "") {
        if (openAccess === true || openAccess.toLowerCase() === "true")
            $("#open-access-filter").addClass("filter-enabled");
        else
            $("#hybrid-filter").addClass("filter-enabled");
    }
    

    initLinkFilter("#InstitutionalDiscounts", "#institutional-discount-filter");
    initLinkFilter("#NoFee", "#no-fee-filter");
    initLinkFilter("#PlanS", "#plan-s-filter");
    initLinkFilter("#Blue", "#blue-journals");
    initLinkFilter("#Lightblue", "#lightblue-journals");
    initLinkFilter("#Grey", "#grey-journals");
}

function initLinkFilter(fieldSelector, linkSelector) {
    var value = $(fieldSelector).val();
    
    if(value)
        $(linkSelector).addClass("filter-enabled");
}

function toggleFilter(filterSelector, fieldSelector, disableSelectors, disableFieldSelectors) {
    $(filterSelector).on("click", function(e) {
        e.preventDefault();
        var $field = $(fieldSelector);
        var currentValue = $field.val();

        if (!currentValue)
            $field.val(true);
        else
            $field.val("");

        $(this).toggleClass("filter-enabled");

        if (!!disableSelectors && !!disableFieldSelectors) {
            $(disableSelectors).removeClass("filter-enabled");
            $(disableFieldSelectors).val("");
        }
        
        
        $("#search-form").submit();
    });
}

function initDisciplinesSelect() {
    $("#SelectedDisciplines").on("chosen:ready", function () {
        $("#loading").hide();
        $("#discipline-container").show();
    }).chosen({
        width: "100%",
        search_contains: true,
        placeholder_text_multiple: "Search by discipline"
        });

    $('#SelectedDisciplines').trigger('chosen:updated');
}