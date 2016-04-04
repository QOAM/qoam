var HomeController = (function () {
    function HomeController() {
    }
    HomeController.prototype.journalScoreCard = function () {
        $('a.jscimage').fancybox();
    };
    HomeController.prototype.index = function() {
        $("#banner").popover({
            container: "body",
            trigger: "hover",
            html: true,
            delay: { "show": 400, "hide": 1250 },
            placement: "bottom",
            content: function() {
                return $("#popover-container").html();
            },
            template: "<div class='popover custom-popover' role='tooltip'><div class='popover-content'></div></div>"
        });
    };
    return HomeController;
})();