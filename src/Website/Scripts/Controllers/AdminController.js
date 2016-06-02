var AdminController = (function () {
    function AdminController() {
    }
    AdminController.prototype.index = function (downloadUrl) {
        $("#download").click(function (e) {
            e.preventDefault();
            $("#donwloadTypeModal").modal()
            .on('hide.bs.modal', function () {
                var data = $("#downloadForm").serialize();
                window.location.href = downloadUrl + "?" + data; 
            });
        });
    };
    return AdminController;
})();