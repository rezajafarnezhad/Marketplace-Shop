$(function () {
    fillDataTable();
    initializingAutocomplete();
    appendHtmlModalPlaceToBody();
});


function getProductDetails(e) {

    var ProductId = $(e).attr('productId');
    GetHtmlWithAjax('?handler=GetProductDetails', { productId: ProductId }, 'ShowProductDetailsInModal', e);
}

function ShowProductDetailsInModal(data, clickedButton) {

    var currnetModal = $('#html-modal-place');
    currnetModal.find('.modal-body').html(data);
    currnetModal.modal('show');
    $('#html-modal-place .modal-header h5').html($(clickedButton).text().trim());
    $.validator.unobtrusive.parse($('#html-modal-place form'));
    initializeTinyMCE();
    activatingDeleteButtons(true);
}


function productStatusInManagingProducts(message, data) {

    showToastr('success', message);
    $('#html-modal-place').modal('hide');
    fillDataTable();
}