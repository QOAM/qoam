var AdminController = (function () {
    var path = window.location.pathname.replace("/removeDuplicates", "");

    function AdminController() {
    }
    AdminController.prototype.index = function (downloadUrl) {
        $("#download").click(function (e) {
            e.preventDefault();
            $("#donwloadTypeModal").modal();

            $("#download-journals").on("click", function () {
                var data = $("#downloadForm").serialize();
                window.location.href = downloadUrl + "?" + data; 
            });
        });
    };

    AdminController.prototype.removeCorner = function() {
        $("#CornerId").chosen({
            search_contains: true
        });

        $("#discard-corner").click(function (e) {
            e.preventDefault();

            var cornerId = $("#CornerId").val();

            if (!cornerId)
                return;
            
            $("#discard-corner-modal").modal();

            $("#confirm-discard").on("click", function () {
                window.location.href = "/corners/" + cornerId + "/delete";
            });
        });
    };

    AdminController.prototype.removeDuplicates = function () {
        $("#remove-duplicates").click(function (e) {
            $(this).hide();
            $("#status-message").show();

            e.preventDefault();

            $.post(path + "/startRemovingDuplicates").done(function() {
                processNextBatch();
            });

            setTimeout(pollRemoveDuplicateCount, 5000);
        });
    };

    function processNextBatch() {
        $.post(path + "/processNextBatch").done(function(response) {
            if (response)
                processNextBatch();
            else
                $("#status-message").text("Completed! ");
        });
    };

    function pollRemoveDuplicateCount() {
        $.get(path + "/removeDuplicateCount").done(function(status) {
            $("#status").text(status);
            setTimeout(pollRemoveDuplicateCount, 5000);
        });
    };

    return AdminController;
})();