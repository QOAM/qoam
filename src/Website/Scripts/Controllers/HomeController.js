var HomeController = (function () {
    function HomeController() {
    }
    HomeController.prototype.journalScoreCard = function () {
        $('a.jscimage').fancybox();
    };
    return HomeController;
})();