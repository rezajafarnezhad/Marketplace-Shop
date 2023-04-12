$(document).on('click', '.increaseProductVariantInCartButton, .decreaseProductVariantInCartButton, .empty-variants-in-cart', function () {

    if ($(this).parents('span').hasClass('text-custom-grey')) {
        return;
    }

    $(this).parent().submit();

});

$(document).ready(function () {
    //$('.delete-Carts-button').click(function () {
    //    var currentForm = $(this).parent();
    //    var customMessage = $(this).attr('custom-message');
    //    const formData = currentForm.serializeArray();
    //    Swal.fire({
    //        title: 'اعلان',
    //        text: customMessage == undefined ? 'آیا مطمئن به حذف سبدخرید هستید ؟' : customMessage,
    //        icon: 'warning',
    //        confirmButtonText: 'بله',
    //        showDenyButton: true,
    //        denyButtonText: 'خیر',
    //        confirmButtonColor: '#067719',
    //        allowOutsideClick: false

    //    }).then((result) => {

    //        if (result.isConfirmed) {
    //            var functionName = 'addProductVariantToCart';
               
    //                alert(currentForm.attr('action')),
    //            $.ajax({

    //                url: currentForm.attr('action'),
    //                type: 'POST',
    //                enctype: 'multipart/form-data',
    //                dataType: 'json',
    //                processData: false,
    //                contentType: false,
    //                success: function (data) {

    //                    if (data.isSuccessful == false) {
    //                        /*var finalData = data.data != null ? data.data : [data.message];*/
    //                        var finalData = data.data || [data.message];
    //                        fillValidationForm(finalData, currentForm);
    //                        showToastr('warning', data.message);
    //                        var modalId = currentForm.parents('.modal').attr('id');
    //                        if (modalId === 'Second-html-modal-place') {

    //                            $('#Second-html-modal-place').modal('show');

    //                        } else if (modalId == 'html-modal-place') {
    //                            $('#html-modal-place').modal('show');

    //                        }
    //                    }
    //                    else {
    //                        window[functionName](data.message, data.data);
    //                    }
    //                },
    //                complete: function () {
    //                    hideLoading();

    //                },
    //                error: function () {
    //                    ShowErrorMessage();
    //                }

    //            });
                
    //        }
    //    });
    //});

});

function addProductVariantToCart(message, data) {

    

    $('#cart-body').html(data.cartbody);
    $('#cart-body .persian-numbers').each(function () {
        var text = $(this).html();
        $(this).html(text.toPersinaDigit())
    });

    var allproductVariants = $('#cart-page-title span').html();
    if (allproductVariants) {
        $('#cart-count-text').html(allproductVariants);

    } else {
        $('#cart-count-text').html('۰');

    }

    enablingNormalTooltips();

}