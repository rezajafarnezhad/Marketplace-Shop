$(function () {
    fillDataTable();
    initializingAutocomplete();
    appendHtmlModalPlaceToBody();
});


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
}


function productStatusInManagingProducts(message, data) {

    showToastr('success', message);
    $('#html-modal-place').modal('hide');
    fillDataTable();
}