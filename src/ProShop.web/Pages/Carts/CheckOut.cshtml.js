


$(function () {

    if ($('#products-box-in-checkout-page > div').length > 1) {

        $("#multipel-shiping-in-checkout-page").removeClass("d-none");
    }

    $('#shipping-dropdown').hover(function () {
        $(this).dropdown('show');

    }, function () {

        $(this).dropdown('hide');

    });

});