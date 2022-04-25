fillDataTable();
appendHtmlModalPlaceToBody();

function getSellerDetails(e) {

    var SellerId = $(e).attr('sellerId');
    GetHtmlWithAjax('?handler=GetSellerDetails', { sellerId: SellerId },'ShowSellerDetailsInModal',e);
}

function ShowSellerDetailsInModal(data, clickedButton) {

    var currnetModal = $('#html-modal-place');
    currnetModal.find('.modal-body').html(data);
    currnetModal.modal('show');
    $('#html-modal-place .modal-header h5').html($(clickedButton).text().trim());
    $.validator.unobtrusive.parse($('#html-modal-place form'));
    initializeTinyMCE();
    activatingDeleteButtons(true);
}


function SellerDocumentInManagingSellers(message,data) {

    showToastr('success', message);
    $('#html-modal-place').modal('hide');
    fillDataTable();
}