

$(function () {

   var variantTitle = $('#variant-box label span').html();


    $('.variant-Item-button').click(function () {

        var selectedVariantId = $(this).attr('veariant-id');
        $("#Variant_VariantId").val(selectedVariantId);
        $('.public-ajax-form').validate().element('#Variant_VariantId');


        var selectedbuttonText = $(this).html();
        $('#variant-box label span').html(`${variantTitle} انتخاب شده :` + selectedbuttonText);


    });

    $('.custom-select2').select2({
        theme:'bootstrap-5',
        ajax: {
            url: location.pathname + '?handler=GetGarantees',
            Delay: 250,
            cash:true,
        },
        placeholder: 'انتخاب کنید',
        minimumInputLength:2
    });
});

function addProductVariantFunc(message, data) {

   
    showToastr('success', message);
    location.href = '/CreateProductVaraintSuccessful';
}