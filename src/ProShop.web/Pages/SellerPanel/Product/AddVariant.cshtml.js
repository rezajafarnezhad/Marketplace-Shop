

$(function () {

   var variantTitle = $('#variant-box label span').html();


    $('.variant-Item-button').click(function () {

        var selectedVariantId = $(this).attr('veariant-id');
        $("#Variant_VariantId").val(selectedVariantId);
        $('.public-ajax-form').validate().element('#Variant_VariantId');


        var selectedbuttonText = $(this).html();
        $('#variant-box label span').html(`${variantTitle} انتخاب شده :` + selectedbuttonText);


    });

});

function addProductVariantFunc(message, data) {

   
    showToastr('success', message);
    location.href = '/CreateProductVaraintSuccessful';
}