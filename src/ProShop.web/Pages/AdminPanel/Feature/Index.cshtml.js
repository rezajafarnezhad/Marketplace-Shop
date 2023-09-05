

$(function () {
    fillDataTable();
    activatingModalForm();
    $('#FeaturesViewModel_SearchFeature_CategoryId').change(function () {

        var categoryId = $(this).val();
        var cate
        if (categoryId == 0) {
            $('#add-feature-for-selected-category').addClass('d-none');
        } else {
            $('#add-feature-for-selected-category').removeClass('d-none');
            var selectedCategoryText = $(this).find('option:selected').text();
            $('#add-feature-for-selected-category').html(`افزودن ویژگی دسته بندی برای "${selectedCategoryText}"`);
            var featureLink = $('#add-feature-for-category').attr('href');
            $('#add-feature-for-selected-category').attr('href', `${featureLink}&categoryId=${categoryId}`);
        }
    });


});


function CreateFeature(message, data) {

    $("#Title").val('');
    $("#Title").focus();
}