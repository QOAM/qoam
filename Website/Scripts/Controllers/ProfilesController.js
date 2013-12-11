var ProfilesController = (function () {
    function ProfilesController() {
    }
    ProfilesController.prototype.index = function (userProfileNamesUrl) {
        $('#Name').typeahead({ prefetch: userProfileNamesUrl });
    };
    return ProfilesController;
})();