$(function () {
    fillDataTable();

    

});

function EditIsColorFunction() {
   
    var IsColor = $('#IsColor').is(':checked');
    $('#IsColor').parents('.form-switch').find('label').html(IsColor ? 'رنگ' : 'سایز');
    $('#IsColor').change(function () {
        var textReplace = this.checked ? 'رنگ' : 'سایز';
        if (this.checked) {
            $('#ColorCodeinput').removeClass('d-none');
            $('#SizeorColor').html("رنگ");
        } else {
            $('#ColorCodeinput').addClass('d-none');
            $('#SizeorColor').html("سایز");
            $('#ColorCode').val();
        }
        $(this).parents('.form-switch').find('label').html(textReplace);
    });
}