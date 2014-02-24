var ProfilesController = (function () {
    function ProfilesController() {
    }
    ProfilesController.prototype.index = function (userProfileNamesUrl) {
        $('#Name').typeahead({ remote: userProfileNamesUrl + '?query=%QUERY' });
    };
    return ProfilesController;
})();