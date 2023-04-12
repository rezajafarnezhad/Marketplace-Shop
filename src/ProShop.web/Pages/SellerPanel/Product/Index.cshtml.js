$(function () {
    fillDataTable();
    initializingAutocomplete();
    appendHtmlModalPlaceToBody();


    $(document).on('blur', '#offPercentage', function () {

        var price = $('#Price').val();
        
        var offPercentaage = $('#offPercentage').val();
        var discountPrice = price / 100 * offPercentaage;
        var priceWithDiscoutn = price - discountPrice;
        $('#OffPrice').val(priceWithDiscoutn);
       
    });


    
});


function activatingDateTimePicker(spanId, inputId) {
    new mds.MdsPersianDateTimePicker(document.getElementById(spanId), {
        targetTextSelector: `#${inputId}`,
        persianNumber: true,
        enableTimePicker: true,
        selectedDate: new Date($(`#${inputId}`).attr('date-en') || new Date()),
        selectedDateToShow: new Date($(`#${inputId}`).attr('date-en') || new Date())
    });
}


function getProductDetails(e) {

    var ProductId = $(e).attr('productId');
    GetHtmlWithAjax('?handler=GetProductDetails', { productId: ProductId }, 'ShowProductDetailsInModal', e);
}

function getProductVariants(e) {
    var ProductId = $(e).attr('productId');
    GetHtmlWithAjax('?handler=GetProductVariants', { productId: ProductId }, 'ShowProductVariantsInModal', e);
}

function ShowProductDetailsInModal(data, clickedButton) {

    var currnetModal = $('#html-modal-place');
    currnetModal.find('.modal-body').html(data);
    currnetModal.modal('show');
    $('#html-modal-place .modal-header h5').html($(clickedButton).text().trim());
    $.validator.unobtrusive.parse($('#html-modal-place form'));
   
}


function ShowProductVariantsInModal(data, clickedButton) {

    var currnetModal = $('#html-modal-place');
    currnetModal.find('.modal-body').html(data);
    currnetModal.modal('show');
    $('#html-modal-place .modal-header h5').html('تنوع های من برای محصول : ' +$(clickedButton).parents('tr').find('td:eq(2)').html());
    $.validator.unobtrusive.parse($('#html-modal-place form'));
    ConvertToPersianNumber();
}


function productStatusInManagingProducts(message, data) {

    showToastr('success', message);
    $('#html-modal-place').modal('hide');
    fillDataTable();
}


function EditProductVariant(e) {
    $('#html-modal-place').modal('hide');
    var pridcutVariantId = $(e).attr('pridcutVariantId');
    GetHtmlWithAjax('?handler=EditProductVariant', { ProdcuctVariantId: pridcutVariantId }, 'ShowEditProductVariantsInModal', e);

}

function ShowEditProductVariantsInModal(data, clickedButton) {
    
    appendSecondHtmlModalPlaceToBody();
    var currnetModal = $('#Second-html-modal-place');
    currnetModal.find('.modal-body').html(data);
    currnetModal.modal('show');
    $('#Second-html-modal-place .modal-header h5').html($(clickedButton).text().trim());
    $.validator.unobtrusive.parse($('#Second-html-modal-place form'));
}

function EditProductVariantFunc(message) {

    
    showToastr('success', message);
}


function EditAddDiscountProductVariant(e) {

    $('#html-modal-place').modal('hide');
    var pridcutVariantId = $(e).attr('pridcutVariantId');
    GetHtmlWithAjax('?handler=AddEditDiscount', { ProdcuctVariantId: pridcutVariantId }, 'ShowEditAddDiscountProductVariantsInModal', e);
}
function ShowEditAddDiscountProductVariantsInModal(data, clickedButton) {

    appendSecondHtmlModalPlaceToBody();
    var currnetModal = $('#Second-html-modal-place');
    currnetModal.find('.modal-body').html(data);
    currnetModal.modal('show');
    $('#Second-html-modal-place .modal-header h5').html($(clickedButton).text().trim());
    $.validator.unobtrusive.parse($('#Second-html-modal-place form'));
    activatingDateTimePicker('startDateTime-AddEditDiscount','StartDateTime');
    activatingDateTimePicker('endDateTime-AddEditDiscount','EndDateTime');
}

function EditAddDiscountProductVariantsFunc(message) {


    showToastr('success', message);
  
}


