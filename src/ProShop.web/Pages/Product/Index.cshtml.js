
function functionNameToCallIntheEnd() {

    var copybtnSelector = $('#copy-product-link-button');
    var btnhtml = copybtnSelector.html();
    copybtnSelector.html('<i class="bi bi-clipboard-check rem20px"></i> کپی شد');
    setTimeout(function () {

        copybtnSelector.html(btnhtml);
    }, 2000);
}


function ProductInfoScrollSpy(e) {


    var scrolltop = $(e).scrollTop();

    $('#product-options-box-in-single-page-of-product > div').removeClass('fw-bold text-danger');
    $('#product-options-box-in-single-page-of-product > div div').addClass('d-none');


    if (scrolltop > $('#question-introduction-in-single-page-product').offset().top - 70) {
        $('#question-introduction-title-single-page-product').addClass('fw-bold text-danger');
        $('#question-introduction-title-single-page-product div').removeClass('d-none');
    }
    else if ($('#commnets-introduction-in-single-page-product').length && scrolltop > $('#commnets-introduction-in-single-page-product').offset().top - 70) {
        $('#comments-introduction-title-single-page-product').addClass('fw-bold text-danger');
        $('#comments-introduction-title-single-page-product div').removeClass('d-none');
    }
    else if ($('#productFeatures-introduction-in-single-page-product').length && scrolltop > $('#productFeatures-introduction-in-single-page-product').offset().top - 70) {
        $('#productFeatures-introduction-title-single-page-product').addClass('fw-bold text-danger');
        $('#productFeatures-introduction-title-single-page-product div').removeClass('d-none');
    }
    else if ($('#specialCheck-introduction-in-single-page-product').length && scrolltop > $('#specialCheck-introduction-in-single-page-product').offset().top - 70) {
        $('#specialCheck-introduction-title-single-page-product').addClass('fw-bold text-danger');
        $('#specialCheck-introduction-title-single-page-product div').removeClass('d-none');
    }
    else {
        $('#product-options-box-in-single-page-of-product > div:first').addClass('fw-bold text-danger');
        $('#product-options-box-in-single-page-of-product > div:first div').removeClass('d-none');

    }
}

$(function () {

    ProductInfoScrollSpy($(this))

    $(document).scroll(function () {
        ProductInfoScrollSpy($(this))
    });

    $("#prodcut-introduction-title-single-page-product").click(function () {

        $('html,body').animate({
            scrollTop: $("#product-introduction-in-single-page-product").offset().top - 69
        }, 0);
    });

    $("#specialCheck-introduction-title-single-page-product").click(function () {

        $('html,body').animate({
            scrollTop: $("#specialCheck-introduction-in-single-page-product").offset().top - 69
        }, 0);
    });

    $("#productFeatures-introduction-title-single-page-product").click(function () {

        $('html,body').animate({
            scrollTop: $("#productFeatures-introduction-in-single-page-product").offset().top - 69
        }, 0);
    });

    $("#comments-introduction-title-single-page-product").click(function () {

        $('html,body').animate({
            scrollTop: $("#commnets-introduction-in-single-page-product").offset().top - 69
        }, 0);
    });

    $("#question-introduction-title-single-page-product").click(function () {

        $('html,body').animate({
            scrollTop: $("#question-introduction-in-single-page-product").offset().top - 69
        }, 0);
    });

    $('.closeandShow').click(function () {

        var isAllFeaturesShown = $(this).find('span.spShow').html().trim() === 'بستن';
        if (isAllFeaturesShown) {
            $(this).find('span.spShow').html('مشاهده بیشتر');
            $("#product-details-in-single-page-of-product > div.d-flex:gt(4)").addClass('d-none');
        } else {
            $(this).find('span.spShow').html('بستن');
            $("#product-details-in-single-page-of-product > div.d-flex").removeClass('d-none');
        }
    });


    if ($('#product-details-in-single-page-of-product').length) {
        var prodcutFeaturesHtml = $('#product-details-in-single-page-of-product').html();
        $('body').append(`<div id="product-features-box-temp" style="visibility:hidden">${prodcutFeaturesHtml}</div>`);
        $('#product-features-box-temp > div.d-flex').removeClass('d-none');

        var theLongestWidthOfProductFeatureDetail = 0;

        $('#product-features-box-temp > div.d-flex').each(function () {

            var CurrentElementWidth = $(this).find('div:first').width();
            if (CurrentElementWidth > theLongestWidthOfProductFeatureDetail) {
                theLongestWidthOfProductFeatureDetail = CurrentElementWidth;
            }
        });

        $('#product-features-box-temp').remove();

        $('#product-details-in-single-page-of-product >div.d-flex').find('div:first').width(theLongestWidthOfProductFeatureDetail);

    }





    var allprodcutCountInCart = $('#cart-dropdown-body div:first').attr('all-product-count-in-cart');
    $('#cart-count-text').html(allprodcutCountInCart.toPersinaDigit());
    $(document).on('click', '.increaseProductVariantInCartButton, .decreaseProductVariantInCartButton, .empty-variants-in-cart', function () {

        if ($(this).parents('span').hasClass('text-custom-grey')) {
            return;
        }

        $(this).parent().submit();

    });



    $('.count-down-timer-in-other-variants').each(function () {
        var currentEl = $(this);
        var selectorToShow = currentEl.parents('td').find('div:first');
        var selectorToHide = currentEl.parents('td').find('div:eq(1)');
        countDownTimer(currentEl, selectorToShow, selectorToHide);
    });

    $('.count-down-timer').each(function () {
        var currentEl = $(this);
        var variantValue = currentEl.parents().attr('variant-value');
        var selectorToShow = $('.product-price-in-single-page-of-product[variant-value="' + variantValue + '"]');
        var selectorToHide = currentEl.parent();
        var selectorToHide2 = $('.product-final-price-in-single-page-of-product[variant-value="' + variantValue + '"]');
        countDownTimer(currentEl, selectorToShow, selectorToHide, selectorToHide2);
    });





    function countDownTimerFunction(selector, selectorToShow, selectorToHide, selectorToHide2, countDownDate) {
        // Get today's date and time
        var now = new Date().getTime();

        // Find the distance between now and the count down date
        var distance = countDownDate - now;

        // Time calculations for days, hours, minutes and seconds
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        var daysText = `${days} روز و<br />`;
        if (days === 0) {
            daysText = '';
        }

        var result =
            `${daysText}${seconds < 10 ? '0' + seconds : seconds} : ${minutes < 10 ? '0' + minutes : minutes} : ${hours < 10 ? '0' + hours : hours}`;

        selector.html(result.toPersinaDigit());

        // If the count down is finished, write some text
        if (distance < 0) {
            selector.parents('tr').attr('is-discount-active', 'false');
            selectorToShow.removeClass('d-none');
            selectorToHide.addClass('d-none');
            if (selectorToHide2) {
                selectorToHide2.addClass('d-none');
            }
        }
        return distance;
    }

    function countDownTimer(selector, selectorToShow, selectorToHide, selectorToHide2) {

        var endDateTime = selector.html().trim();

        // Set the date we're counting down to
        var countDownDate = new Date(endDateTime).getTime();

        countDownTimerFunction(selector, selectorToShow, selectorToHide, selectorToHide2, countDownDate);

        // Update the count down every 1 second
        var x = setInterval(function () {
            var result = countDownTimerFunction(selector, selectorToShow, selectorToHide, selectorToHide2, countDownDate);
            if (result < 0) {
                clearInterval(x);
            }
        }, 1000);
    }





    $('#other-sellers-count-box').click(function () {
        $('html, body').animate({

            scrollTop: $('#other-sellers-box').offset().top - 20

        }, 2);

    });



    $('#copy-product-link-button').click(function () {

        var productLink = $(this).attr("product-Link");
        copyTextToClipboard(productLink, 'functionNameToCallIntheEnd');
    });


    var zoomPluginOptions = {
        fillContainer: true,
        zoomPosition: 'original'
    };

    new ImageZoom(document.getElementById('zoom-image-place'), zoomPluginOptions);

    $('#add-product-to-favorite-form').submit(function () {

        if (!IsAuthenticated) {
            showFirstLoginModal();
            return false;
        }

    });

    $('#ShareProductBtn').click(function () {

        $('#share-product-modal').modal('show');

    });



    if ($('.other-sellers-table:first tbody tr').length === 1) {

        $('#other-sellers-box').addClass('d-none');


    } else {

    }


    $("#ShowAllProductFeatures").click(function () {
        $(this).addClass("d-none");
        $("#ProductFeaturesBox li").removeClass("d-none");
    });


    $('#product-variants-box-in-show-product-info  div:first').addClass('selected-variant-in-show-product-info');
    $('#product-variants-box-in-show-product-info  i:first').removeClass('d-none');

    // change color

    $('#product-variants-box-in-show-product-info div').click(function () {

        if ($(this).find('i').hasClass('d-none') === false) {
            return;
        }


        $('#product-variants-box-in-show-product-info div').removeClass('selected-variant-in-show-product-info');
        $('#product-variants-box-in-show-product-info i').addClass('d-none');

        $(this).find('i').removeClass('d-none');
        $(this).addClass('selected-variant-in-show-product-info');


        var selectedVariantValue = $(this).attr('aria-label');

        var selectedProductVariantId = $(this).attr('product-variant-id');

        changeVariant(selectedVariantValue, selectedProductVariantId);



    });


    // change size

    $('#product-variants-box-in-show-product-info select').change(function () {

        var selectedVariantValue = this.value;
        var selectedProductVariantId = $(this).find(':selected').attr('product-variant-id');
        changeVariant(selectedVariantValue, selectedProductVariantId);

    });



    function changeVariant(selectedVariantValue, selectedProductVariantId) {

        $('.other-sellers-table').addClass('d-none');

        $('.product-final-price-in-single-page-of-product').addClass('d-none');
        $('.product-price-in-single-page-of-product').addClass('d-none');
        $('.Product-discount-box').addClass('d-none');


        $('.other-sellers-table[variant-value="' + selectedVariantValue + '"]').removeClass('d-none');
        if ($('.other-sellers-table[variant-value="' + selectedVariantValue + '"]').length === 0) {

            $('#other-sellers-box').addClass('d-none');
        } else {
            $('#other-sellers-box').removeClass('d-none');

        }

        // change Variant Value
        $('#Product-variant-Value').html(selectedVariantValue);

        //change Product Info In left Side box
        var selectedSeller = $('.other-sellers-table[variant-value="' + selectedVariantValue + '"] tbody tr:first');

        if (selectedSeller.attr('is-discount-active') === "true") {

            $('.product-final-price-in-single-page-of-product[variant-value="' + selectedVariantValue + '"]').removeClass('d-none');
            $('.Product-discount-box[variant-value="' + selectedVariantValue + '"]').removeClass('d-none');

        } else {

            $('.product-price-in-single-page-of-product[variant-value="' + selectedVariantValue + '"]').removeClass('d-none');
        }




        //change ShopName
        var selectedShopName = selectedSeller.find("td:first").text();
        $('#shop-details-in-page-of-product div').text(selectedShopName);
        $('#ShopNameForInfo').text(selectedShopName);


        var tooltip = bootstrap.Tooltip.getInstance('#product-shopName-tooltip');
        tooltip.setContent({ '.tooltip-inner': `"این کالا توسط فروشنده آن ${selectedShopName.trim()}، قیمت گذاری شده است."` });

        //change Seller Logo
        var selectedLogo = selectedSeller.find('td:first i').length === 0 ? 'img' : 'i';
        if (selectedLogo === 'img') {
            selectedLogo = selectedSeller.find('td:first img').attr('src');
            $('#shop-details-in-page-of-product i').addClass('d-none');
            $('#shop-details-in-page-of-product img').removeClass('d-none');
            $('#shop-details-in-page-of-product img').attr('src', selectedLogo);


        } else {
            $('#shop-details-in-page-of-product i').removeClass('d-none');
            $('#shop-details-in-page-of-product img').addClass('d-none');

        }

        //change GaranteeName
        var selectedGaranteeName = selectedSeller.find("td:eq(1)").text();
        $('#garanteeName').text(selectedGaranteeName);


        //change score
        var selectedscore = selectedSeller.find("td:eq(2)").html();
        $('#scoreProduct').html(selectedscore);

        //show or hide other selllers box
        var otherSellersCount = $('.other-sellers-table[variant-value="' + selectedVariantValue + '"] tbody tr').length;
        if (otherSellersCount === 1) {
            $('#other-sellers-box').addClass('d-none');
            $('#other-sellers-count-box').addClass('d-none');
        } else {
            $('#other-sellers-box').removeClass('d-none');
            $('#other-sellers-count-box').removeClass('d-none');
        }

        //change other Sellers count
        $('#other-sellers-count-box span').html(otherSellersCount - 1);


        //show delivery Box

        if (selectedSeller.attr('free-delivery') === 'true') {
            $('#free-delivery-box').removeClass('d-none');

        } else {
            $('#free-delivery-box').addClass('d-none');
        }


        $('.latest-product-stock-in-inventory').addClass('d-none');
        $('.latest-product-stock-in-inventory[variant-value="' + selectedVariantValue + '"]').removeClass('d-none');



        //Change cart Count

        $('#product-info-left-side-box .product-variant-in-cart-section').addClass('d-none');

        $('.product-variant-in-cart-section[variant-id="' + selectedProductVariantId + '"]').addClass('d-none');

        var cartSectionEl = $('#product-info-left-side-box .product-variant-in-cart-section[variant-id="' + selectedProductVariantId + '"]');


        if (cartSectionEl.find('.productvariantcountincart span:first').text().trim() !== '۰') {
            cartSectionEl.removeClass('d-none');
            $('.product-variant-in-cart-section[variant-id="' + selectedProductVariantId + '"]').removeClass('d-none');

        }
        $('#product-info-left-side-box .add-product-variant-to-cart').addClass('d-none');
        $('.add-product-variant-to-cart[variant-id="' + selectedProductVariantId + '"]').addClass('d-none');

        //change buttonCart

        // $('#product-info-left-side-box .add-product-variant-to-cart').addClass('d-none');
        if (cartSectionEl.find('.productvariantcountincart span:first').text().trim() === '۰') {
            $('.add-product-variant-to-cart[varaint-id="' + selectedProductVariantId + '"]').removeClass('d-none');
        }
    }

});


function addFavoriteFunc() {

    var addFavoriteBtn = $("#AddFavoritebtn").parent().find('input[name="addFavorite"]');

    if (addFavoriteBtn.val() === 'true') {

        addFavoriteBtn.val('false');

        $('#AddFavoritebtn i:first').addClass('d-none')
        $('#AddFavoritebtn i:last').removeClass('d-none')
        showToastr('success', "به لیست علاقه مندی اضافه شد");


    } else {
        addFavoriteBtn.val('true');

        $('#AddFavoritebtn i:first').removeClass('d-none')
        $('#AddFavoritebtn i:last').addClass('d-none')
    }
}


function addProductVariantToCart(message, data) {

    var addProductVariantToCartEl = $('.add-product-variant-to-cart[varaint-id="' + data.productvariantid + '"]');
    var currentSectionEl = $('.product-variant-in-cart-section[variant-id="' + data.productvariantid + '"]');



    currentSectionEl.find('.productvariantcountincart span:first').html(data.count.toString().toPersinaDigit());


    if (data.iscartfull) {

        currentSectionEl.find('.productvariantcountincart span:last').removeClass('d-none');
        currentSectionEl.find('.increaseProductVariantInCartButton').parents('span').addClass('text-custom-grey');
        currentSectionEl.find('.increaseProductVariantInCartButton').parents('span').removeClass('pointer-cursor');

    } else {
        currentSectionEl.find('.productvariantcountincart span:last').addClass('d-none');
        currentSectionEl.find('.increaseProductVariantInCartButton').parents('span').removeClass('text-custom-grey');
        currentSectionEl.find('.increaseProductVariantInCartButton').parents('span').addClass('pointer-cursor');
    }

    debugger;
    var selectedProductVariantId = 0;
    var selectedColor = parseInt($('#product-variants-box-in-show-product-info div i').not('[class*="d-none"]').parents('div').attr('product-variant-id'));
    var selectedSize = parseInt($('#product-variants-box-in-show-product-info select').find(':selected').attr('product-variant-id'));
    if (selectedColor || selectedSize) {
        selectedProductVariantId = parseInt(selectedColor || selectedSize);
    } else {
        selectedProductVariantId = data.selectedProductVariantId
    }


    if (selectedProductVariantId === data.productvariantid) {
        currentSectionEl.addClass('d-none');
        addProductVariantToCartEl.addClass('d-none');

        if (data.count > 0) {
            currentSectionEl.removeClass('d-none');


        } else {
            addProductVariantToCartEl.removeClass('d-none');

        }
    }




    if (data.count === 1) {
        currentSectionEl.find('.decreaseProductVariantInCartButton').parents('span').addClass('d-none');
        currentSectionEl.find('.empty-variants-in-cart').parents('span').removeClass('d-none');
    } else if (data.count > 1) {
        currentSectionEl.find('.empty-variants-in-cart').parents('span').addClass('d-none');
        currentSectionEl.find('.decreaseProductVariantInCartButton').parents('span').removeClass('d-none');
    }
    $('#cart-dropdown-body').html(data.cartsDetails);
    $('#cart-dropdown-body .persian-numbers').each(function () {
        var text = $(this).html();
        $(this).html(text.toPersinaDigit())
    });


    var allprodcutCountInCart = $('#cart-dropdown-body div:first').attr('all-product-count-in-cart');
    $('#cart-count-text').html(allprodcutCountInCart.toPersinaDigit());


}

function commentReportsFunc(message) {
    showToastr('success', message);
}


function showCommentsByPagination(el) {

    if ($(el).hasClass("bg-danger")) {
        return;
    }

    var productId = $(".container-fluid[product-id]").attr('product-id');
    var commentPagesCount = $(".container-fluid[comment-Pages-Count]").attr('comment-Pages-Count');;
    var pageNumber = $(el).attr("page-number");
    var sortBy = $("#comments-sorting div.text-danger").attr('sort-by');
    var orderBy = $("#comments-sorting div.text-danger").attr('order-by');
    var dataToSend = {
        productId: productId,
        pageNumber: pageNumber,
        commentPagesCount: commentPagesCount,
        sortBy: sortBy,
        orderBy: orderBy
    };

    GetHtmlWithAjax('?handler=ShowCommentsByPagination', dataToSend, 'showCommentByPagingFunction');
}

$("#comments-sorting div.pointer-cursor").click(function () {

    if ($(this).hasClass("text-danger")) {
        return;
    }

    $("#comments-sorting div.pointer-cursor").removeClass("text-danger");
    $("#comments-sorting div.pointer-cursor").addClass("text-secondary");
    $(this).addClass("text-danger");
    var productId = $(".container-fluid[product-id]").attr('product-id');
    var commentPagesCount = $(".container-fluid[comment-Pages-Count]").attr('comment-Pages-Count');;
    var pageNumber = 1;
    var sortBy = $(this).attr('sort-by');
    var orderBy = $(this).attr('order-by');
    var dataToSend = {
        productId: productId,
        pageNumber: pageNumber,
        commentPagesCount: commentPagesCount,
        sortBy: sortBy,
        orderBy: orderBy
    };

    GetHtmlWithAjax('?handler=ShowCommentsByPagination', dataToSend, 'showCommentByPagingFunction');
});

function showCommentByPagingFunction(data) {

    $("#comment-box-in-single-page-of-product").html(data);
    ConvertToPersianNumber();
    scorollTo('#commnets-introduction-in-single-page-product', 69);
}

$(document).on('click', '.comment-score-form-submit', function () {
    $(this).submit();
});

function commentReportsFunc(message, data, form) {

    var isLiskclicked = $(form).find('i').hasClass('bi-hand-thumbs-up') || $(form).find('i').hasClass('bi-hand-thumbs-up-fill');
    scorollTo('#commnets-introduction-in-single-page-product', 69);

    var currentSpan = $(form).find('span');
    var currentScoreValue = parseInt(currentSpan.html().trim().toEnglishDigit());

    if (data === 'Add') {
        var valueToRplase = (currentScoreValue + 1).toString().toPersinaDigit();
        currentSpan.html(valueToRplase);
        if (isLiskclicked) {
            $(form).find('i').removeClass('bi-hand-thumbs-up');
            $(form).find('i').addClass('text-success bi-hand-thumbs-up-fill');
        } else {
            $(form).find('i').removeClass('bi-hand-thumbs-down');
            $(form).find('i').addClass('text-danger bi-hand-thumbs-down-fill');
        }

    } else if (data === 'Subtract') {
        var valueToRplase = (currentScoreValue - 1).toString().toPersinaDigit();
        currentSpan.html(valueToRplase);
        if (isLiskclicked) {
            $(form).find('i').removeClass('text-seuccess bi-hand-thumbs-up-fill');
            $(form).find('i').addClass('bi-hand-thumbs-up');
        } else {
            $(form).find('i').removeClass('text-danger bi-hand-thumbs-down-fill');
            $(form).find('i').addClass('bi-hand-thumbs-down');
        }

    } else {
        var valueToRplase = (currentScoreValue + 1).toString().toPersinaDigit();
        currentSpan.html(valueToRplase);

        if (isLiskclicked) {
            $(form).find('i').removeClass('bi-hand-thumbs-up');
            $(form).find('i').addClass('text-success bi-hand-thumbs-up-fill');
        } else {
            $(form).find('i').removeClass('bi-hand-thumbs-down');
            $(form).find('i').addClass('text-danger bi-hand-thumbs-down-fill');
        }

        var anotherForm;
        if (isLiskclicked) {
            anotherForm = $(form).parent().find('form:last');
            anotherForm.find('i').removeClass('text-danger bi-hand-thumbs-down-fill');
            anotherForm.find('i').addClass('bi-hand-thumbs-down');
        } else {
            anotherForm = $(form).parent().find('form:first');
            anotherForm.find('i').removeClass('text-seuccess bi-hand-thumbs-up-fill');
            anotherForm.find('i').addClass('bi-hand-thumbs-up');
        }
        var anotherSpan = anotherForm.find('span');
        var anotherScoreValue = parseInt(anotherSpan.html().trim().toEnglishDigit());
        anotherSpan.html((anotherScoreValue - 1).toString().toPersinaDigit());
    }
}