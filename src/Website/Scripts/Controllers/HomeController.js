﻿var HomeController = (function () {
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

        // store that the user has seen the In Memoriam for Leo.
        localStorage.setItem("inmemleo", "true");
    };

    HomeController.prototype.demoPlanS = function() {
        $("#InstitutionId").chosen({
            search_contains: true
        });
    };
    return HomeController;
})();