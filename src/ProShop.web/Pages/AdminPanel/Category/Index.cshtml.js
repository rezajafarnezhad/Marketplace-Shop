

$(function () {

    function activatingDeleteButtons() {
        $('.delete-row-button').click(function () {
            var currentForm = $(this).parent();
            var customMessage = $(this).attr('custom-message');
            Swal.fire({
                title: 'اعلان',
                text: customMessage == undefined ? 'آیا مطمئن به حذف هستید ؟' : customMessage,
                icon: 'warning',
                confirmButtonText: 'بله',
                showDenyButton: true,
                denyButtonText: 'خیر',
                confirmButtonColor: '#067719',
                allowOutsideClick: false

            }).then((result) => {

                if (result.isConfirmed) {
                    var data = {
                        elementId: currentForm.find('input:first').val(),
                        __RequestVerificationToken: currentForm.find('input:last').val(),
                    }
                    showLoading();
                    $.post(currentForm.attr('action'), data, function (data, status) {
                        if (data.isSuccessful == false) {
                            showToastr('warning', data.message);
                        } else {
                            fillDataTable();
                            showToastr('success', data.message);
                        }

                    }).always(function () {
                        hideLoading();
                    }).fail(function () {
                        ShowErrorMessage();
                    })
                }
            });
        });

    }



    function activatingModalForm() {
        $('.show-modal-form-button').click(function (e) {
            e.preventDefault();
            var Url = $(this).attr('href');
            var customtitle = $(this).attr('custom-title');
            if (customtitle == undefined) {
                customtitle = $(this).text().trim();
            }
            $('#form-modal-place .modal-header h5').html(customtitle);
            showLoading();
            $.get(Url, function (data, status) {
                if (data.isSuccessful == false) {

                    showToastr('warning', data.message);
                } else {

                    $('#form-modal-place .modal-body').html(data);
                    $.validator.unobtrusive.parse($('#form-modal-place form'));
                    initializeTinyMCE();
                    initializeSelect2();
                    $("#form-modal-place").modal("show");
                }
            }).fail(function () {
                ShowErrorMessage();
            }).always(function () {
                hideLoading();
            });
        });

    }

    //activatingModalForm();

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
        $('.read-data-table .data-table-body').remove();
        $('.search-form-loading').attr('disabled', 'disabled');
        $('.data-table-loading').removeClass('d-none');

        const formData = $('form.Search-form-via-ajax').serializeArray();

        $.get(`${location.pathname}?handler=GetDataTable`, formData, function (data, status) {
            $('.Search-form-loading').removeAttr('disabled');
            $('.data-table-loading').addClass('d-none');

            if (status == 'success') {
                $('.read-data-table').append(data);
                activatingPagination();
                activatingGotoPage();
                activatingModalForm();
                activatingPageCount();
                activatingDeleteButtons();
                enablingTooltips();
            }
            else {
                ShowErrorMessage()
            }
        });
    }

    fillDataTable();


    function activatingPageCount() {

        $('#page-count-selectbox').change(function () {

            var pageCountValue = this.value;
            $('.Search-form-via-ajax input[name$="Pagination.PageCount"]').val(pageCountValue);
            $('.Search-form-via-ajax').submit();
        });
    }


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
        $('[data-bs-toggle="tooltip"], .tooltip').tooltip("hide");
        $('#record-not-found-box').remove();
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
                    $('.read-data-table').append(data);
                    activatingPagination();
                    activatingGotoPage();
                    activatingDeleteButtons();
                    activatingPageCount();
                    activatingModalForm();
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



