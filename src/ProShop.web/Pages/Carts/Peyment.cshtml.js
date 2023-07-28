$(function () {

    $('#shipping-dropdown').hover(function () {
        $(this).dropdown('show');

    }, function () {

        $(this).dropdown('hide');

    });

});



$('form').submit(function () {
    $('#btn-pay').attr('disabled', 'disabled');
});