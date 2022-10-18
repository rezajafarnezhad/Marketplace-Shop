$(function () {
    fillDataTable();
    initializingAutocomplete();
    appendHtmlModalPlaceToBody();
    const dtp1Instance = new mds.MdsPersianDateTimePicker(document.getElementById('delvery-date-in-Search-Consignment'), {
        targetTextSelector: '#Consignment_SearchConsignments_DeliveryDate',
        persianNumber: true,
    });
});



function getConsignmentDetails(e) {

    var ConsignmentId = $(e).attr('consignmentId');
    GetHtmlWithAjax('?handler=GetConsignmentDetails', { consignmentId: ConsignmentId }, 'ShowConsignmentDetailsInModal', e);
}

function ShowConsignmentDetailsInModal(data, clickedButton) {

    var currnetModal = $('#html-modal-place');
    currnetModal.find('.modal-body').html(data);
    currnetModal.modal('show');
    $('#html-modal-place .modal-header h5').html($(clickedButton).text().trim());
    $.validator.unobtrusive.parse($('#html-modal-place form'));
}

function ChangeConsignmentStatus(e) {

    var ConsignmentId = $(e).attr('consignmentId');
    GetHtmlWithAjax('?handler=GetChangeConsignmentStatus', { consignmentId: ConsignmentId }, 'ShowChangeConsignmentStatusInModal', e);
}

function ShowChangeConsignmentStatusInModal(data, clickedButton) {

    var currnetModal = $('#html-modal-place');
    currnetModal.find('.modal-body').html(data);
    currnetModal.modal('show');
    $('#html-modal-place .modal-header h5').html($(clickedButton).text().trim());
    initializeTinyMCE();
    $.validator.unobtrusive.parse($('#html-modal-place form'));
}


function ConfirmationConsignmentFunc(message, data) {

    showToastr('success', message);
    $('#html-modal-place').modal('hide');
    fillDataTable();
}

function consignmentReceivedAndAddStockSuccess(message, data) {

    showToastr('success', message);
    $('#html-modal-place').modal('hide');
    fillDataTable();
}