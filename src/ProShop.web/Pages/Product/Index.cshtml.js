
function functionNameToCallIntheEnd() {

    var copybtnSelector = $('#copy-product-link-button');
    var btnhtml = copybtnSelector.html();
    copybtnSelector.html('<i class="bi bi-clipboard-check rem20px"></i> کپی شد');
    setInterval(function () {

        copybtnSelector.html(btnhtml);
    }, 2000);
}


$(function () {


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
        var selectorToHide2 = $('.product-final-price-in-single-page-of-product[variant-value="' + variantValue +'"]');
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
            selector.parents('tr').attr('is-discount-active','false');
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


        $('#product-variants-box-in-show-product-info div').removeClass('selected-variant-in-show-product-info ');
        $('#product-variants-box-in-show-product-info i').addClass('d-none');

        $(this).find('i').removeClass('d-none');
        $(this).addClass('selected-variant-in-show-product-info ');


        var selectedVariantValue = $(this).attr('aria-label');
        changeVariant(selectedVariantValue);

    });


    // change size

    $('#product-variants-box-in-show-product-info select').change(function () {

        var selectedVariantValue = this.value;
        changeVariant(selectedVariantValue);

    });



    function changeVariant(selectedVariantValue) {

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