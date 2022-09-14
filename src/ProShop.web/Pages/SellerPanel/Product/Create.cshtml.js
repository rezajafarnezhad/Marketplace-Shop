
activatingModalForm();
function getCategories() {

    GetHtmlWithAjax(`${location.pathname}?handler=GetCategories`, null, 'showCategories', null);
}
$(function () {

    //Disabled all tabs except first
    $('#add-product-tab button:not(:first)').attr('disabled', 'disabled');
    $('#add-product-tab button:not(:first)').addClass('not-allowed-cursor');

    $(document).on('click', '.btn-next-tab', function () {

        var nextTabId = $(this).parents('.tab-pane').next().attr('id');
        $(`#add-product-tab button[data-bs-target="#${nextTabId}"]`).tab('show');

    });



    getCategories();

    var specialtyCheckTinyMce = tinymce.get('Product_SpecialtyCheck');
    specialtyCheckTinyMce.settings.max_height = 1000;



    $("#Product_ProductImageFiles").on("change", function () {
        if ($("#Product_ProductImageFiles")[0].files.length > 10) {
            ShowMessageErrorForUploadFiles('شما مجاز به آپلود حداکثر 10 تصویر برای محصول میباشید.','RemoveImages');
        }
    });

    $("#Product_ProductVideoFiles").on("change", function () {
        if ($("#Product_ProductVideoFiles")[0].files.length > 3) {
            ShowMessageErrorForUploadFiles('شما مجاز به آپلود حداکثر 3 ویدیو برای محصول میباشید.','RemoveFiles');
        }
    });



});


function RemoveImages() {

    $("#product-images-preview-box").html('');
    $("#Product_ProductImageFiles").val('');
    $('[id^="image-preview-box-temp"]').remove();

}
function RemoveFiles() {

    $("#Product_ProductVideoFiles").val('');

}



var selectedCategoriesIds = [];

function showCategories(data) {
    $('#product-category div.card-body.row').html(data);
    $('#selected-categories-for-add-product').html('');
    selectedCategoriesIds.forEach(element => {
        var currentCategory = $('#product-category button[category-Id=' + element + ']');
        currentCategory.addClass('active');
        var currentCategoryText = currentCategory.text().trim();
        $('#selected-categories-for-add-product').append(
            `<span> ${currentCategoryText} <i class="bi bi-chevron-left"></i></span>`
        );
    });
    $('#product-category div.card-body button[has-child=true]').click(function () {
        $('#select-product-category-button').attr('disabled', 'disabled');
        $('#select-product-category-button').addClass('btn-light');
        $('#select-product-category-button').removeClass('btn-primary');

        var selectedRow = parseInt($(this).parent().attr('category-row'));
        for (var counter = selectedRow; counter <= selectedCategoriesIds.length; counter++) {

            $('#product-category div[category-row=' + counter + '] button').removeClass('active');
        }
        $('#product-category div.card-body button[has-child=false]').removeClass('active');
        $(this).addClass('active');
        selectedCategoriesIds = [];
        $('#product-category button.active').each(function () {
            selectedCategoriesIds.push($(this).attr('category-Id'));
        });
        GetHtmlWithAjax(`${location.pathname}?handler=GetCategories`, { selectedCategoriesIds: selectedCategoriesIds }, 'showCategories', null);
    });

    $('#product-category div.card-body button[has-child=false]').click(function () {
        var selectedRow = parseInt($(this).parent().attr('category-row'));
        $('#product-category div[category-row=' + selectedRow + '] button').removeClass('active');
        for (var counter = selectedRow; counter <= selectedCategoriesIds.length; counter++) {
            $('#product-category div[category-row=' + (counter + 1) + ']').remove();
        }

        $(this).addClass('active');
        $('#selected-categories-for-add-product').html('');
        $('#product-category button.active').each(function () {
            var currentCategory = $(this);
            var currentCategoryText = currentCategory.text().trim();
            if (currentCategory.attr('has-child') === 'true') {
                $('#selected-categories-for-add-product').append(
                    `<span> ${currentCategoryText} <i class="bi bi-chevron-left"></i></span>`
                );
            } else {
                $('#selected-categories-for-add-product').append(
                    `<span> ${currentCategoryText}</span>`
                );
            }
        });
        $('#select-product-category-button').removeAttr('disabled');
        $('#select-product-category-button').removeClass('btn-light');
        $('#select-product-category-button').addClass('btn-primary');
    });

    $('#reset-product-category-button').click(function () {
        if ($('#selected-categories-for-add-product span').length > 0) {
            $('#select-product-category-button').attr('disabled', 'disabled');
            $('#select-product-category-button').addClass('btn-light');
            $('#select-product-category-button').removeClass('btn-primary');
            selectedCategoriesIds = [];
            getCategories();
        }

    });
}

var requestNewBrandUrl = $('#request-new-brand-url').attr('href');
var IsCategoryAlreadySelected = false;
var categoryId;
$('#select-product-category-button').click(function () {

    if (IsCategoryAlreadySelected) {

        ShowSweetAlert2('تغییر دسته بندی منجر به از بین رفتن تمامی اطلاعات وارد شده شما میشود، آیا مطمئن به انجام این کار هستید ؟', 'emptyAllInputsAndShowOtherTabs', 'undoSelectedCategoryButton');

    } else {
        emptyAllInputsAndShowOtherTabs();
    }

    IsCategoryAlreadySelected = true;


});

function resetinputs() {

    RemoveImages();
    RemoveFiles();
    $("#Create-Product-Form input").not(`[name="${rvt}"] , #Product_MainCategoryId`).val('');

    tinymce.get("Product_SpecialtyCheck").setContent("<p>بررسی تخصصی محصول</p>");
    tinymce.get("Product_ShortDescription").setContent("<p>توضیحات کوتاه محصول</p>");



}

function emptyAllInputsAndShowOtherTabs() {

    resetinputs();
    categoryId = $('#product-category div.list-group.col-3:last button.active').attr('category-Id');
    getDateWithAjax(`${location.pathname}?handler=CategoryInfo`, { categoryId: categoryId }, 'CategoryInfo');
    $('#request-new-brand-url').attr('href', requestNewBrandUrl + '&categoryId=' + categoryId);
    $('#Product_MainCategoryId').val(categoryId);
}


function undoSelectedCategoryButton() {

    $(`#product-category button`).removeClass("active");
    $(`#product-category button[category-id="${categoryId}"]`).addClass("active");

}


function CategoryInfo(message, data) {

    //showCategoryBrands

    $('#add-product-tab button[data-bs-target="#product-info"]').tab('show');

    $('#Product_BrandId option').remove();

    $('#Product_BrandId').append('<option value="0">انتخاب کنید</option>');

    $.each(data.brands, function (Key, value) {
        $('#Product_BrandId').append(`<option value="${Key}">${value}</option>`);
    })

    //End showCategoryBrands

    //changeIsFakeStatus


    if (data.canAddFakeProduct === false) {
        $('#Product_IsFake').attr('disabled', 'disabled');
        $('#Product_IsFake').prop('checked', false);
    } else {

        $('#Product_IsFake').removeAttr('disabled');
    }

    //End changeIsFakeStatus

    // showCategoryFeatures

    $('#Productfeatures .card-body.row').html(data.categoryFeaturs);

    // End showCategoryFeatures
    initializeSelect2WithoutModal();

    //Active all tabs and remove allowed cursor
    $('#add-product-tab button:not(:first)').removeAttr('disabled');
    $('#add-product-tab button:not(:first)').removeClass('not-allowed-cursor');


}

function RequestForAddBrandFunction() {
    var IsIranianBrand = $('#IsIranianBrand').is(':checked');
    $('#IsIranianBrand').parents('.form-switch').find('label').html(IsIranianBrand ? 'ایرانی' : 'خارجی');
    $('#IsIranianBrand').change(function () {
        var textReplace = this.checked ? 'ایرانی' : 'خارجی';
        $(this).parents('.form-switch').find('label').html(textReplace);
    });
}


function CreateProduct(message, data) {

    
    showToastr('success', message);
   location.href = '/SellerPanel/Product/CreateProductSuccessful';

}