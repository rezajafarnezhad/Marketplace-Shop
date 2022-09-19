
$(function () {
    fillDataTable();

});

var brandBox =
    ` <div class="btn-group mb-3">
                <button type="button" class="btn btn-outline-dark btn-sm">
                   [brand title]
                </button>
                <button type="button" class="btn btn-success btn-sm">
                   %
                  [commission percentage]
                </button> 

                  <button type="button" class="btn btn-danger btn-sm remove-selected-brands">
                    <i class="bi bi-x-lg"></i>
                </button>
            </div>`;


function onAutocompleteSelect(event, ui) {

    var enteredBrand = ui.item.value;
    var commissionpercentage = $('#commission-percentage-input').val();
   
    if (isNullOrWhitespace(commissionpercentage)) {
        showToastr('error','درصد کمیسیون را وارد کنید')
        return;
    }
    var parsedcommissionpercentage = parseInt(commissionpercentage);
    if (parsedcommissionpercentage > 25 || parsedcommissionpercentage < 1) {
        showToastr('error', 'درصد کمیسیون باید بین 1 تا 25 درصد باشد');
        return;
    }
    if ($('#Add-Brand-To-Category-Form input[type="hidden"][value^="' + enteredBrand + '"]').length == 0) {
        var brandBoxToAppend = brandBox.replace('[brand title]', enteredBrand);
        brandBoxToAppend = brandBoxToAppend.replace('[commission percentage]', commissionpercentage);
        $('#empty-selected-brands').addClass('d-none');
        $('#selected-brands-box').append(brandBoxToAppend);
        event.preventDefault();
        $(event.target).val('');
        var InputToAppend = `<input type="hidden" name="Brands" value="${enteredBrand}|||${commissionpercentage}" />`
        $('#Add-Brand-To-Category-Form').prepend(InputToAppend);
        showToastr('info', 'برند با موفقیت اضافه شد');
    } else {
        showToastr('warning', 'این برند قبلا ثبت شده');

    }
    event.preventDefault();
    $(event.target).val('');
}

$(document).on('click', '.remove-selected-brands', function () {

    var selectedBrandText = $(this).parent().find('button:first').text().trim();
    $(this).parent().remove();
    $('#Add-Brand-To-Category-Form input[value^="' + selectedBrandText + '"]').remove();
    showToastr('info', 'برند با موفقیت حذف شد');
    if ($('#selected-brands-box .btn-group').length === 0) {
        $('#empty-selected-brands').removeClass('d-none');
    }
});

$(document).on('keydown', "#search-brand", function () {

    if (event.key == 'Enter') {

        event.preventDefault();
    }
});