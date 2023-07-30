$(function () {
    fillDataTable();
   
    const dtp1Instance = new mds.MdsPersianDateTimePicker(document.getElementById('order-date-in-Search-Orders'), {
        targetTextSelector: '#ShowOrders_SearchOrders_CreatedDateTime',
        persianNumber: true,
        
    });

    $('#ShowOrders_SearchOrders_ProvinceId').change(function () {

        var formdata = {
            ProvinceId: $(this).val()
        }

        getDateWithAjax('/Inventory/Order/Index?handler=GetCities', formdata, 'putCitiesInTheSelectBox');

    });

    $(document).on('click', '.copy-post-tracking-code-button', function () {
        var postTrackingCode = $(this).attr('post-tracking-code');
        copyTextToClipboard(postTrackingCode, 'copyParcelPostLinkToClipboardFunction', $(this));
    });

});
function putCitiesInTheSelectBox(message, data) {

    $('#selectCity').removeClass('d-none');

    $('#ShowOrders_SearchOrders_CityId option').remove();

    $('#ShowOrders_SearchOrders_CityId').append('<option value="0">انتخاب کنید</option>');

    $.each(data, function (Key, value) {
        $('#ShowOrders_SearchOrders_CityId').append(`<option value="${Key}">${value}</option>`);
    })
}


function copyParcelPostLinkToClipboardFunction(clickedEl) {
    $(clickedEl).find('i').addClass('d-none');
    $(clickedEl).find('span:last').removeClass('d-none');

    // این فانکشن فقط یکبار فراخوانی میشود
    // و بهترین گزینه برای این سناریو می باشد
    setTimeout(function () {
        $(clickedEl).find('i').removeClass('d-none');
        $(clickedEl).find('span:last').addClass('d-none');
    }, 2000);
}