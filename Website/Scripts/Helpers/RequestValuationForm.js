$(document).ready(function() {
   $('#ModelMessage').fadeIn("slow");

   $('#MailToAddress').focus(function() {
        if ($('#ModelMessage').is(":visible")) {
            $('#ster').fadeOut("slow");
            $('#ModelMessage').fadeOut("slow");
        }
    });
})