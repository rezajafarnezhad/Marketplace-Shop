$(function () {
    fillDataTable();

    $('#ShowConstantValues_SearchFeatureConstant_CategoryId').change(function () {

        var CategoryId = $(this).val();
        getDateWithAjax(`${location.pathname}?handler=GetCategoryFeatures`, { categotyId: CategoryId }, "fillFeaturesInSelectBox");
    });

    //Select Box In Modal

    $(document).on('change',"#CategoryId", function () {
        var CategoryId = $(this).val();
        getDateWithAjax(`${location.pathname}?handler=GetCategoryFeatures`, { categotyId: CategoryId }, "fillFeaturesInSelectBoxInModal");
    });

});


function fillFeaturesInSelectBoxInModal(message, data) {

    $('#FeatureId option').remove();

    $('#FeatureId').append('<option value="0">انتخاب کنید</option>');

    $.each(data, function (Key, value) {
        $('#FeatureId').append(`<option value="${Key}">${value}</option>`);
    })
}

function fillFeaturesInSelectBox(message, data) {

    $('#ShowConstantValues_SearchFeatureConstant_FeatureId option').remove();

    $('#ShowConstantValues_SearchFeatureConstant_FeatureId').append('<option value="0">انتخاب کنید</option>');

    $.each(data, function (Key, value) {
        $('#ShowConstantValues_SearchFeatureConstant_FeatureId').append(`<option value="${Key}">${value}</option>`);
    })
}

