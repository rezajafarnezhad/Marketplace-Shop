fillDataTable();
appendFormModalPlaceToBody();


function actionsAfterLoadModalForm() {
    $('#IsIranianBrand').change(function () {
        var textReplace = this.checked ? 'ایرانی' : 'خارجی';
        $(this).parents('.form-switch').find('label').html(textReplace);
    });
}