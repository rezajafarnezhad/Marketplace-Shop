
$(function () {

    $('.show-modal-form-button').click(function (e) {
        e.preventDefault();
        var Url = $(this).attr('href');
        showLoading();
        $.get(Url, function (data, status) {
            hideLoading();
            if (status == 'success') {
                $('#form-modal-place .modal-body').html(data);
                $.validator.unobtrusive.parse($('#form-modal-place form'));
                initializeTinyMCE();
                initializeSelect2();
                $("#form-modal-place").modal("show");
            }
            else {
                ShowErrorMessage()
            }
        });

    });


    var isMainPaginationClicked = false;
    var isGotoPageClicked = false;

    $(document).on('submit', 'form.custom-ajax-form', function (e) {

        e.preventDefault();
        var currentForm = $(this);
        var formAction = currentForm.attr("action");
        var formdata = new FormData(this);
        $.ajax({

            url: formAction,
            data: formdata,
            type: 'POST',
            enctype: 'multipart/form-data',
            dataType: 'json',
            processData: false,
            contentType: false,
            beforeSend: function () {

                currentForm.find('.submit-custom-ajax-button span').removeClass('d-none');
                currentForm.find('.submit-custom-ajax-button').attr('disabled', 'disabled');

            },
            success: function (data, status) {

                if (data.isSuccessful == false) {
                    fillValidationForm(data.data, currentForm);
                    showToastr('warning', data.message);
                } else {
                    fillDataTable();
                    $("#form-modal-place").modal("hide");
                    showToastr('success', data.message);
                }
            },
            complete: function () {

                currentForm.find('.submit-custom-ajax-button span').addClass('d-none');
                currentForm.find('.submit-custom-ajax-button').removeAttr('disabled');

            },
            error: function () {
                ShowErrorMessage();
            }

        });
    });


    function activatingPagination() {
        $("#main-pagianation button").click(function () {
            isMainPaginationClicked = true;
            var currentPageSelected = $(this).val();
            $('.Search-form-via-ajax input[name$="Pagination.CurrentPage"]').val(currentPageSelected);
            $('.Search-form-via-ajax').submit();
        });
    }

    function activatingGotoPage() {
        $("#goto-page-button").click(function () {
            isGotoPageClicked = true;

        });
    }

    function fillDataTable() {
        $.get(`${location.pathname}?handler=GetDataTable`, function (data, status) {
            $('.Search-form-loading').removeAttr('disabled');
            $('.data-table-loading').addClass('d-none');

            if (status == 'success') {
                $('.read-data-table .data-table-body').remove();
                $('.read-data-table').append(data);
                activatingPagination();
                activatingGotoPage();
                enablingTooltips();
            }
            else {
                ShowErrorMessage()
            }
        });
    }

    fillDataTable();

    $(document).on('submit', 'form.Search-form-via-ajax', function (e) {
        e.preventDefault();
        var currentForm = $(this);

        var pageNumberInput = $('#page-number-input').val();
        if (isGotoPageClicked || $('#page-number-input').is(':focus')) {
            $('.Search-form-via-ajax input[name$="Pagination.CurrentPage"]').val(pageNumberInput);
        }
        else if (!isMainPaginationClicked) {
            $('.Search-form-via-ajax input[name$="Pagination.CurrentPage"]').val(1);
        } 

        const formData = currentForm.serializeArray();

        //show loaging and disabling button

        currentForm.find('.Search-form-loading').attr('disabled', 'disabled');
        currentForm.find('.Search-form-loading span').removeClass('d-none');

        $('.data-table-loading').removeClass('d-none');
        $('.data-table-body').html('');


        $.get(`${location.pathname}?handler=GetDataTable`, formData, function (data, status) {
            isMainPaginationClicked = false;
            isGotoPageClicked = false;
            //Hide loaging and Activating button

            currentForm.find('.Search-form-loading').removeAttr('disabled', 'disabled');
            currentForm.find('.Search-form-loading span').addClass('d-none');
            $('.data-table-loading').addClass('d-none');


            if (status == 'success') {
                if (data.isSuccessful == false) {
                    fillValidationForm(data.data, currentForm);
                    showToastr('warning', data.message);
                } else {
                    $('.read-data-table .data-table-body').html(data);
                    activatingPagination();
                    activatingGotoPage();
                    enablingTooltips();
                }

            } else {
                ShowErrorMessage()
            }
        });

    });

    function fillValidationForm(errors, currentForm) {
        var result = '<ul>';
        errors.forEach(function (e) {
            result += `<li>${e}</li>`;
        });
        result += '</ul>';
        currentForm.find('div[class*="validation-summary"]').html(result);

    }

});



