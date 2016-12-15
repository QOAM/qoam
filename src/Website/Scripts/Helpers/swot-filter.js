function initSwotFilter() {
    $("#swotFilterContainer").on("click", "div.table-cell", function () {
        $(this).toggleClass("verdict-" + $(this).attr("data-swot-type")).toggleClass("selected");
        return false;
    });

    $("input[type='submit']").on("click", function () {
        var swotInput = $("#SwotMatrix");
        swotInput.val("");

        $("#swotFilterContainer").find("div.selected").each(function () {
            var currentValue = swotInput.val();
            var swotType = $(this).attr("data-swot-type");

            if (currentValue === "") {
                swotInput.val(swotType);
            } else {
                swotInput.val(currentValue + "," + swotType);
            }
        });
    });
}