var AdminController = (function () {
    function AdminController() {
    }
    AdminController.prototype.index = function (downloadUrl) {
        $("#download").click(function (e) {
            e.preventDefault();
            $("#donwloadTypeModal").modal();
        });
    };
    return AdminController;
})();