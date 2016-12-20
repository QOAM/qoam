var AdminController = (function () {
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

    return AdminController;
})();