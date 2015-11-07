var ProfilesController = (function () {
    function ProfilesController() {
    }
    ProfilesController.prototype.index = function (userProfileNamesUrl) {
        createTypeahead('#Name', userProfileNamesUrl);
    };
    return ProfilesController;
})();