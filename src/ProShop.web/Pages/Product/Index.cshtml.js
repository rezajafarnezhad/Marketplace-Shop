

$(function () {


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

        $('#product-variants-box-in-show-product-info div').removeClass('selected-variant-in-show-product-info ');
        $('#product-variants-box-in-show-product-info i').addClass('d-none');

        $(this).find('i').removeClass('d-none');
        $(this).addClass('selected-variant-in-show-product-info ');


        var selectedVariantValue = $(this).attr('data-bs-original-title');
        $('.other-sellers-table').addClass('d-none');
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
        //change ShopName
        var selectedShopName = selectedSeller.find("td:first").text();
        $('#shop-details-in-page-of-product div').text(selectedShopName);

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

        //change Price
        var selectedPrice = selectedSeller.find("td:eq(3)").text();
        $('#PriceProduct').text(selectedPrice);

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

    });


    // change size

    $('#product-variants-box-in-show-product-info select').change(function () {

        var selectedVariantValue = this.value;
        $('.other-sellers-table').addClass('d-none');
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
        //change ShopName
        var selectedShopName = selectedSeller.find("td:first").text();
        $('#shop-details-in-page-of-product div').text(selectedShopName);

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

        //change Price
        var selectedPrice = selectedSeller.find("td:eq(3)").text();
        $('#PriceProduct').text(selectedPrice);

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

    });

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
        showToastr('warning', "از لیست علاقه مندی حذف شد");
    }
}