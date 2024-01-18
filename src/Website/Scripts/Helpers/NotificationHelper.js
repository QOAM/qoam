function initializeInMemoriamNotification() {
    var notificaitonSeen = sessionStorage.getItem("notification-seen") === "true";
    var inMemoriamSeen = localStorage.getItem("inmemleo") === "true";

    if (inMemoriamSeen || notificaitonSeen)
        return;

    $("#in-memoriam-modal").modal();

    $("#dismiss-inmemoriam-notification").on("click", function () {
        sessionStorage.setItem("notification-seen", true);
    });
}