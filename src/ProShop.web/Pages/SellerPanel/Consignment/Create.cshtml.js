
function appendProductVariantTr(data) {
    $('#record-not-found-box').addClass('d-none');
    $('#consignment-items').append(data);
    $('#consignment-items tr:last').find('input').attr('max', maxCount);

    var variantCode = $('#consignment-items tr:last').attr('variant-Code');
    $('#variant-Code-items-form-in-create-consignment').prepend(

        `<input type="hidden" value="${variantCode}|||1"/>`

    )

    $('#send-consignment-submit-button').removeAttr('disabled');
}


$(function () {
    $(document).on("click", '.remove-product-variant-tr', function () {
        var currentVariantCode = $(this).parents('tr').attr('variant-Code');
        $('#variant-Code-items-form-in-create-consignment input[value="' + currentVariantCode + '"]').remove();
        $('#variant-Code-items-form-in-create-consignment input[value^="' + currentVariantCode + '|||"]').remove();
        $(this).parents('tr').remove();
        if ($('#consignment-items tr').length === 0) {
            $('#record-not-found-box').removeClass('d-none');
            $('#send-consignment-submit-button').attr('disabled', 'disabled');

        }
    });



    const dtp1Instance = new mds.MdsPersianDateTimePicker(document.getElementById('delvery-date-in-Create-Consignment'), {
        targetTextSelector: '#CreateConsignment_DeliveryDate',
        persianNumber: true,
    });

    $('.get-html-by-sending-form').submit(function () {
        var selectedVariantCode = $('#VariantCode').val();
        if ($('#variant-Code-items-form-in-create-consignment input:hidden[value^="' + selectedVariantCode + '|||"]').length > 0) {
            showToastr('warning', "این تنوع محصول از قبل اضافه شده")
            return false;
        }
    });

    $('#variant-Code-items-form-in-create-consignment').submit(function () {


        $(this).find('input:hidden').not('input[name="' + rvt + '"]').remove();
        $(this).find('table tbody tr').each(function () {
            var currentVariantCode = $(this).attr('variant-Code');
            var currentProductCount = $(this).find('input').val();

            var parsedProductCount = parseInt(currentProductCount);
            if (parsedProductCount > maxCount || parsedProductCount < 1) {
                showToastr('warning', `تعداد هر محصول باید بین 1 تا ${maxCount} باشد`)
                return false;
            }

            $('#variant-Code-items-form-in-create-consignment').prepend(
                `<input type="hidden" name="CreateConsignment.Variants" value="${currentVariantCode}|||${currentProductCount}"/>`
            )
        });

    });


});

var maxCount = 100000;



function CreateConsignment(message, data) {

    alert("s");
    showToastr('success', message);
    // location.href ="" ;
}