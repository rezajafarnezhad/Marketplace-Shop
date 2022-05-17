﻿
activatingModalForm();
function getCategories() {

    GetHtmlWithAjax(`${location.pathname}?handler=GetCategories`, null, 'showCategories', null);
}
$(function () {

    getCategories();

    var specialtyCheckTinyMce = tinymce.get('Product_SpecialtyCheck');
    specialtyCheckTinyMce.settings.max_height = 1000;

});

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

$('#select-product-category-button').click(function () {

  var categoryId = $('#product-category div.list-group.col-3:last button.active').attr('category-Id');
    getDateWithAjax(`${location.pathname}?handler=GetCategoryBrands`, { categoryId: categoryId},'showCategoryBrands' )
    getDateWithAjax(`${location.pathname}?handler=CanAddFakeProduct`, { categoryId: categoryId }, 'changeIsFakeStatus')
    GetHtmlWithAjax(`${location.pathname}?handler=GetCategoryFeatures`, { categoryId: categoryId }, 'showCategoryFeatures')
    $('#request-new-brand-url').attr('href', requestNewBrandUrl + '&categoryId=' + categoryId);

});


function showCategoryBrands(message,data) {

    $('#add-product-tab button[data-bs-target="#product-info"]').tab('show');

    $('#Product_BrandId option').remove();

    $('#Product_BrandId').append('<option value="0">انتخاب کنید</option>');

    $.each(data, function (Key, value) {
        $('#Product_BrandId').append(`<option value="${Key}">${value}</option>`);
    })
}

function changeIsFakeStatus(message,data) {

    if (data === false) {
        $('#Product_IsFake').attr('disabled', 'disabled');
        $('#Product_IsFake').prop('checked', false);
    } else {

        $('#Product_IsFake').removeAttr('disabled');
    }
}

function RequestForAddBrandFunction() {
    var IsIranianBrand = $('#IsIranianBrand').is(':checked');
    $('#IsIranianBrand').parents('.form-switch').find('label').html(IsIranianBrand ? 'ایرانی' : 'خارجی');
    $('#IsIranianBrand').change(function () {
        var textReplace = this.checked ? 'ایرانی' : 'خارجی';
        $(this).parents('.form-switch').find('label').html(textReplace);
    });
}


function showCategoryFeatures(data) {

    $('#Productfeatures .card-body.row').html(data);

}


function CreateProduct(message,data) {
}