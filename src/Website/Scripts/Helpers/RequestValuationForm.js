$(document).ready(function() {
   $('#ModelMessage').fadeIn("slow");

   $('#MailToAddress').focus(function() {
        if ($('#ModelMessage').is(":visible")) {
            $('#ster').fadeOut("slow");
            $('#ModelMessage').fadeOut("slow");
        }
   });

   $("#toggle-editor").click(function () {
       $(this).slideUp("fast");
       $("#custom-text-info, #info-text").slideUp("fast");
       $("#collapsible-panel").slideToggle("fast");
   });

    $("#custom-text-info").click(function(e) {
        e.preventDefault();
        $("#info-text").slideDown("fast");

        setTimeout(function () { $("#info-text").slideUp(); }, 7000);
    });
})