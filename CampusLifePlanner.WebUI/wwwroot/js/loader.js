$(window).on('load', function () {
    $("#preloader .inner").fadeOut();
    $("#preloader").delay(350).fadeOut('slow');
    $('body').delay(350).css({ 'overflow': 'visible' });
});

LoadButtomStart();

$(document).ajaxStart(function () {
    $("#preloader .inner").show();
    $("#preloader").show();
    $('body').css({ 'overflow': 'visible' });
    LoadButtomStart();
});
$(document).ajaxStop(function () {
    $("#preloader .inner").fadeOut();
    $("#preloader").delay(350).fadeOut('slow');
    $('body').delay(350).css({ 'overflow': 'visible' });
    LoadButtomStop();
});

function LoadButtomStart() {
    $(".fp").click(function () {
        $(this).find(".spinner-border, .visually-hidden").show();
        $(this).find(".bi").hide();
        $(this).prop("disabled", true);
    });
}
function LoadButtomStop() {
    setTimeout(function () {
        $(".fp").find(".spinner-border, .visually-hidden").hide();
        $(".fp").find(".bi").show();
        $(".fp").prop("disabled", false);
    }, 350);
}