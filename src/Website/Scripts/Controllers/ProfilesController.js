var ProfilesController = (function () {
    function ProfilesController() {
    }
    ProfilesController.prototype.index = function (userProfileNamesUrl) {
        window.createTypeahead('#Name', userProfileNamesUrl);
        $("#Institution").chosen({
            search_contains: true
        });

        $(".delete-profile").click(function (e) {
            e.preventDefault();

            var $this = $(this);
            var deleteUrl = this.href;
            var profileName = $this.data("profile-name");

            $("#profile-name").html(profileName);
            $("#delete-profile-modal").modal();

            $("#confirm-delete-profile").on("click", function () {
                window.location.href = deleteUrl;
            });
        });
    };
    return ProfilesController;
})();