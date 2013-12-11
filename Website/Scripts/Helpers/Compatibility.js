$(function() {
    if (!Modernizr.input.placeholder) {
        $('.editor-label-placeholder label').show();
    }
    
    $('a.external').prop('target', '_blank');
})