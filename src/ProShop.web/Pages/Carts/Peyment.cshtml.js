$(function () {

    $('#shipping-dropdown').hover(function () {
        $(this).dropdown('show');

    }, function () {

        $(this).dropdown('hide');

    });

});


function createOrderAndPayFunc(message, data) {
    location.href = data;
}