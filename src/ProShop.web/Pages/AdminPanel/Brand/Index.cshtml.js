$(function () {
    fillDataTable();

});

function EditBrandFunction() {
    var IsIranianBrand = $('#IsIranianBrand').is(':checked');
    $('#IsIranianBrand').parents('.form-switch').find('label').html(IsIranianBrand ? 'ایرانی' : 'خارجی');
    $('#IsIranianBrand').change(function () {
        var textReplace = this.checked ? 'ایرانی' : 'خارجی';
        $(this).parents('.form-switch').find('label').html(textReplace);
    });
}


function ConfirmAndRejectBrand(message) {

    showToastr('success', message);
    $('#form-modal-place').modal('hide');
    fillDataTable();
}