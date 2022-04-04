

$(function () {



    $.get(`${location.pathname}?handler=GetDataTable`, function (data, stauts) {
        if (stauts == 'success') {
            $('.read-data-table').html(data);
        }
        else {
            ShowErrorMessage()
        }
    });



    $(document).on('submit', 'form.Search-form-via-ajax', function (e) {
        e.preventDefault();
        var currentForm = $(this);
        const formData = currentForm.serializeArray();
        $.get(`${location.pathname}?handler=GetDataTable`, formData, function (data, stauts) {
            if (stauts == 'success') {
                if (data.isSuccessful == false) {
                    var errors = '<ul>';
                    data.data.forEach(function (e) {
                        errors += `<li>${e}</li>`;
                    });
                    errors += '</ul>';
                    currentForm.find('div[class*="validation-summary"]').html(errors);
                    showToastr('warning', data.message);
                } else {
                    $('.read-data-table').html(data);

                }

            } else {
                ShowErrorMessage()
            }
        });

    });


});



