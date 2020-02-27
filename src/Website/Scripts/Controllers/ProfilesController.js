var ProfilesController = (function () {
    function ProfilesController() {
    }
    ProfilesController.prototype.index = function (userProfileNamesUrl) {
        createTypeahead('#Name', userProfileNamesUrl);
        $("#Institution").chosen({
            search_contains: true
        });
    };
    return ProfilesController;
})();