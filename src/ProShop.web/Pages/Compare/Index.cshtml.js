
var timeOut = null;
var pageNumber = 1;
var isLastPage = false;
var isProcessing = false;
var searchValue = '';
var searchViaInput = false;

var isModalOpened = false;


//Search
$(document).on('keyup', '#search-input-in-Compare-page', function () {

    if (timeOut != null) {
        clearTimeout(timeOut);
    }
    timeOut = setTimeout(function () {

        if (searchValue !== $("#search-input-in-Compare-page").val().trim()) {
            searchValue = $("#search-input-in-Compare-page").val().trim();
            $("#search-input-in-Compare-page").attr('disabled', 'disabled');
            $("#add-product-header-in-compare-page").addClass('d-none');
            $('#add-product-modal-in-compare-page').html('');
            isProcessing = true;
            searchViaInput = true;
            $('#compare-partilal-loading').removeClass('d-none');

            if (searchValue === '') {

                GetHtmlWithAjax('?handler=ShowAddProductForCompare', { productCodeToHide: getProductCodesToHide() }, 'ShowAddProductInModal', null, false);
            } else {
                var dataTosend = {
                    searchValue: searchValue,
                    productCodeToHide: getProductCodesToHide()
                }
                pageNumber = 1;
                GetHtmlWithAjax('?handler=ShowAddProductForCompare', dataTosend, 'ShowAddProductInModal', null, false);
            }
        }


    }, 1000);

});





function ShowAddProductInModal(data, clickedButton) {

    isProcessing = false;
    isModalOpened = true;

    appendHtmlScrollableModalPlaceToBody('modal-lg');

    $('#html-scrollable-modal-place .modal-body').off('scroll').scroll(function (e) {

        var contentHeight = this.scrollHeight;

        // تعیین ارتفاع قابل مشاهده
        var visibleHeight = this.clientHeight;

        // تعیین میزان اسکرول از بالا
        var scrollFromTop = this.scrollTop + 30;

        // اگر به آخر المحتوا رسیدیم و هنوز صفحات دیگری وجود دارند
        if (scrollFromTop + visibleHeight >= contentHeight && !isLastPage && isProcessing === false) {

            isProcessing = true;
            // ارسال درخواست AJAX
            var dataTosend = {
                pageNumber: ++pageNumber,
                productCodeToHide: getProductCodesToHide(),
                searchValue: searchValue
            }
            GetHtmlWithAjax('?handler=ShowAddProductForCompare', dataTosend, 'ShowAddProductInModal', e, false);
        }

    });

    var currnetModal = $('#html-scrollable-modal-place');


    if (data.data.pageNumber === 1) {
        currnetModal.find('.modal-body').html(data.data.productBody);
    }
    else {
        currnetModal.find('.modal-body #add-product-modal-in-compare-page').append(data.data.productBody);
        $('#Product-Count-In_Compare-Page').html(data.data.productCount);
    }

    isLastPage = data.data.isLastPage;

    if (data.data.isLastPage) {
        $('#compare-partilal-loading').addClass('d-none');
    } else {
        $('#compare-partilal-loading').removeClass('d-none');

    }

    $("#search-input-in-Compare-page").val(searchValue);
    if (searchViaInput) {
        searchViaInput = false;
        $("#search-input-in-Compare-page").focus();
    }
    currnetModal.modal('show');
    $('#html-scrollable-modal-place .modal-header h5').html('انتخاب کالا برای مقایسه');
    ConvertToPersianNumber();

}


//حذف محصول از لیست مقایسه
$(document).on('click', '.remove-button-in-compare-page', function () {

    $(this).parents('.product-item-in-compare-page').remove();
    var selectedProductCode = $(this).attr('product-code');
    var ProductCode1 = $('.product-item-in-compare-page').eq(0).attr('product-code');
    var ProductCode2 = $('.product-item-in-compare-page').eq(1).attr('product-code');
    var ProductCode3 = $('.product-item-in-compare-page').eq(2).attr('product-code');
    var datatosend = {
        productCode1: ProductCode1, productCode2: ProductCode2, productCode3: ProductCode3
    };
    isModalOpened = false;

    GetHtmlWithAjax('/Compare/Index?handler=GetProductsForCompare', datatosend, 'ShowProductsInCompareFunction', null);

});

///کلبک روی مخصولات داخل مدال
$(document).on('click', '#add-product-modal-in-compare-page a', function (e) {

    e.preventDefault();
    if (isProcessing) {
        return;
    }


    var selectedProductCode = $(this).attr('product-code');
    var ProductCode1 = $('.product-item-in-compare-page').eq(0).attr('product-code');
    var ProductCode2 = $('.product-item-in-compare-page').eq(1).attr('product-code');
    var ProductCode3 = $('.product-item-in-compare-page').eq(2).attr('product-code');
    var prodcutcode4 = selectedProductCode;

    var datatosend = {
        productCode1: ProductCode1, productCode2: ProductCode2, productCode3: ProductCode3, productCode4: prodcutcode4
    };
    isModalOpened = false;
    closeScrollableModal();
    GetHtmlWithAjax('/Compare/Index?handler=GetProductsForCompare', datatosend, 'ShowProductsInCompareFunction', e);

});

//محصولات درون لیست مقایسه دیگر در مدال نمایش داده نشوند
function getProductCodesToHide() {

    var ProductCode1 = $('.product-item-in-compare-page').eq(0).attr('product-code');
    var ProductCode2 = $('.product-item-in-compare-page').eq(1).attr('product-code');
    var ProductCode3 = $('.product-item-in-compare-page').eq(2).attr('product-code');

    var productCodesToHide = [];

    productCodesToHide.push(ProductCode1);
    productCodesToHide.push(ProductCode2);
    productCodesToHide.push(ProductCode3);
    return productCodesToHide;
}


function ShowAddProductForCompare(e) {
    if (isModalOpened === false) {
        GetHtmlWithAjax('?handler=ShowAddProductForCompare', { productCodeToHide: getProductCodesToHide() }, 'ShowAddProductInModal', e);
    } else {
        var currnetModal = $('#html-scrollable-modal-place');
        currnetModal.modal('show');
    }
}

function ShowProductsInCompareFunction(result) {
    $('#Compare-page').html(result);
    if ($('.product-item-in-compare-page').length === 1) {
        $('.product-item-in-compare-page:first').find('div:first').addClass('invisible');
    }
    ConvertToPersianNumber();

}