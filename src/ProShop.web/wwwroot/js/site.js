
var rvt = '__RequestVerificationToken';


var htmlModalPlace = `<div class="modal fade" id="html-modal-place" data-bs-backdrop="static">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title"></h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body"></div>
            <div class="modal-footer d-flex justify-content-start">
                <button type="button" class="btn btn-danger" data-bs-dismiss="modal">بستن</button>
            </div>
        </div>
    </div>
</div>`;

function appendHtmlModalPlaceToBody() {
    if ($('#html-modal-place').length === 0) {
        $('body').append(htmlModalPlace);
    }
}

var formModalPlace = `<div class="modal fade" id="form-modal-place" data-bs-backdrop="static">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title"></h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body"></div>
            <div class="modal-footer d-flex justify-content-start">
                <button type="button" class="btn btn-danger" data-bs-dismiss="modal">بستن</button>
            </div>
        </div>
    </div>
</div>`;

function appendFormModalPlaceToBody() {
    if ($('#form-modal-place').length === 0) {
        $('body').append(formModalPlace);
    }
}




var loadingModalHtml = `<div class="modal" id="loading-modal" data-bs-backdrop="static">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">لطفا صبر کنید</h5>
            </div>
            <div class="modal-body text-center">
                <img src="/images/application/loading.gif" />
            </div>
        </div>
    </div>
</div>`;


function showLoading() {
    if ($('#loading-modal').length === 0) {
        $('body').append(loadingModalHtml);
    }
    $('#loading-modal').modal('show');
}
function hideLoading() {
    $('#loading-modal').modal('hide');
}
// Toastr

function showToastr(status, message) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    toastr[status](message);
}

// End toastr


//ToolTip

function enablingTooltips() {

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

function ShowErrorMessage(message) {

    showToastr('error', message != null ? message : 'خطایی به وجود آمد، لطفا مجددا تلاش نمایید');
}


// Send `TinyMCE` images to server with specific url
function sendTinyMceImagesToServer(blobInfo, success, failure, progress, url) {
    var formData = new FormData();
    formData.append('file', blobInfo.blob(), blobInfo.filename());
    formData.append(rvt, $('textarea.custom-tinymce:first').parents('form').find('input[name="' + rvt + '"]').val());
    $.ajax({
        url: `${location.pathname}?handler=${url}`,
        data: formData,
        type: 'POST',
        enctype: 'multipart/form-data',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (data) {
            if (data === false) {
                failure('خطایی به وجود آمد');
            } else {
                success(data.location);
            }
        },
        error: function () {
            failure('خطایی به وجود آمد');
        }
    });
};
function initializeTinyMCE() {

    $('textarea.custom-tinymce').each(function() {

        var textareaId = `#${$(this).attr('id')}`;
        if ($('textarea.custom-tinymce').length > 0) {
            tinymce.remove(textareaId);
            tinymce.init({
                selector: textareaId,
                setup: function (editor) {
                    editor.on('blur', function (e) {
                        var elementId = $(e.target.targetElm).attr('id');
                        $(e.target.formElement).validate().element(`#${elementId}`);
                    });
                },
                min_height: 300,
                max_height: 500,
                language: 'fa_IR',
                language_url: '/js/fa_IR.js',
                content_style: 'body {font-family: Vazir}',
                plugins: 'link table preview wordcount autoresize',
                toolbar: [
                    {
                        name: 'history', items: ['undo', 'redo', 'preview']
                    },
                    {
                        name: 'styles', items: ['styleselect']
                    },
                    {
                        name: 'formatting', items: ['bold', 'italic', 'underline', 'link']
                    },
                    {
                        name: 'alignment', items: ['alignleft', 'aligncenter', 'alignright', 'alignjustify', 'forecolor', 'backcolor']
                    },
                    {
                        name: 'table', items: ['table', 'wordcount']
                    },
                    {
                        name: 'indentation', items: ['outdent', 'indent']
                    }
                ],
                // menubar: false,
                branding: false
            });
        }
    });

 

}



document.addEventListener('focusin', function (e) {
    if (e.target.closest('.tox-tinymce-aux, .moxman-window, .tam-assetmanager-root') !== null) {
        e.stopImmediatePropagation();
    }
});

function initializeSelect2() {
    if ($('.modal .custom-select2').length > 0) {
        $('.modal .custom-select2').select2({
            theme: 'bootstrap-5',
            dropdownParent: $('#form-modal-place'),

        });
    }
}
function initializeSelect2WithoutModal() {

    if ($('.custom-select2').length > 0) {
        $('.custom-select2').select2({
            theme: 'bootstrap-5',
        });
    }

}


// Validation

// fileRequired




var imageInputsWithProblems = [];

// یک آرایه و یک آیتم میگیره
// آیتم رو از آرایه حذف میکنه
function removeItemInArray(arr, item) {
    var found = arr.indexOf(item);

    while (found !== -1) {
        arr.splice(found, 1);
        found = arr.indexOf(item);
    }
}



if (jQuery.validator) {
    // برای اعتبار سنجی اینپوت های مخفی از این کد استفاده میکنیم
    // کجا کاربر داره ؟
    // هنگام استفاده از
    // navs-tabs
    // What is navs-tabs ?
    // https://getbootstrap.com/docs/5.0/components/navs-tabs/#javascript-behavior
    // برای مثال در تب دوم، اینپوت های تب اول مخفی هستن
    // و به صورت پیشفرض اعتبار سنجی نمیشن
    // کد پایین، اینپوت های مخفی رو هم اعتبار سنجی میکنه
    $.validator.setDefaults({
        ignore: [],
        // other default options
    });

    var defaultRangeValidator = $.validator.methods.range;

    $.validator.methods.range = function (value, element, param) {
        if (element.type === 'checkbox') {
            return element.checked;
        } else {
            return defaultRangeValidator.call(this, value, element, param);
        }
    }

    jQuery.validator.addMethod("fileRequired", function (value, element, param) {
        if (element.files[0] != null)
            return element.files[0].size > 0;
        return false;
    });
    jQuery.validator.unobtrusive.adapters.addBool("fileRequired");

    // allowExtensions
    jQuery.validator.addMethod('allowExtensions', function (value, element, param) {
        if (element.files[0] != null) {
            var whiteListExtensions = $(element).data('val-whitelistextensions').split(',');
            return whiteListExtensions.includes(element.files[0].type);
        }
        return true;
    });
    jQuery.validator.unobtrusive.adapters.addBool('allowExtensions');

    // isImage
    jQuery.validator.addMethod('isImage', function (value, element, param) {
        var selectedFile = element.files[0];
        if (selectedFile === undefined) {
            return true;
        }
        var whiteListExtensions = $(element).data('val-whitelistextensions').split(',');
        if (!whiteListExtensions.includes(selectedFile.type)) {
            return false;
        }
        var currentElementId = $(element).attr('id');
        var currentForm = $(element).parents('form');

        if (imageInputsWithProblems.includes(currentElementId)) {
            removeItemInArray(imageInputsWithProblems, currentElementId);
            return false;
        }

        if ($('#image-preview-box-temp').length === 0) {
            $('body').append('<img class="d-none" id="image-preview-box-temp" />');
        }
        $('#image-preview-box-temp').attr('src', URL.createObjectURL(selectedFile));
        $('#image-preview-box-temp').off('error');
        $('#image-preview-box-temp').on('error',
            function () {
                imageInputsWithProblems.push(currentElementId);
                currentForm.validate().element(`#${currentElementId}`);
            });
        return true;
    });
    jQuery.validator.unobtrusive.adapters.addBool('isImage');


    // maxFileSize
    jQuery.validator.addMethod('maxFileSize', function (value, element, param) {
        if (element.files[0] != null) {
            var maxFileSize = $(element).data('val-maxsize');
            var selectedFileSize = element.files[0].size;
            return maxFileSize >= selectedFileSize;
        }
        return true;
    });
    jQuery.validator.unobtrusive.adapters.addBool('maxFileSize');

    // makeTinyMceRequired
    jQuery.validator.addMethod('makeTinyMceRequired', function (value, element, param) {
        var editorId = $(element).attr('id');
        var editorContent = tinyMCE.get(editorId).getContent();
        $('body').append(`<div id="test-makeTinyMceRequired">${editorContent}</div>`);
        var result = isNullOrWhitespace($('#test-makeTinyMceRequired').text());
        $('#test-makeTinyMceRequired').remove();
        return !result;
    });
    jQuery.validator.unobtrusive.adapters.addBool('makeTinyMceRequired');
}


// End validation
function isNullOrWhitespace(input) {

    if (typeof input === 'undefined' || input == null) return true;

    return input.replace(/\s/g, '').length < 1;
}


//Ajax operations


// فعال ساز دکمه حذف، در داخل گرید
function activatingDeleteButtons(isModalmode) {
    $('.delete-row-button').click(function () {
        var currentForm = $(this).parent();
        var customMessage = $(this).attr('custom-message');
        const formData = currentForm.serializeArray();
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

                showLoading();
                $.post(currentForm.attr('action'),
                    formData,
                    function (data, status) {
                        if (data.isSuccessful == false) {
                            showToastr('warning', data.message);
                        } else {
                            if (isModalmode) {
                                $('#html-modal-place').modal('hide');
                            }
                            showToastr('success', data.message);
                            fillDataTable();
                        }

                    }).always(function () {
                        hideLoading();
                    }).fail(function () {
                        ShowErrorMessage();
                    });
            }
        });
    });

}


function initializingAutocomplete() {
    if ($('.autocomplete').length > 0) {
        $('.autocomplete').autocomplete({
            source: `${location.pathname}?handler=AutocompleteSearch`,
            minLength: 2,
            delay: 500,
            select: function(event, ui) {
                window['onAutocompleteSelect'](event , ui);

            }
        });
    }


}


// این فانکشن فرم های مربوط به ایجاد و ویرایش را
// به صورت ایجکس برگشت میزند که در داخل مودال نمایش دهیم
function activatingModalForm() {
    $('.show-modal-form-button').click(function (e) {
        e.preventDefault();
        var Url = $(this).attr('href');
        var customtitle = $(this).attr('custom-title');
        if (customtitle == undefined) {
            customtitle = $(this).text().trim();
        }
        appendFormModalPlaceToBody();
        $('#form-modal-place .modal-header h5').html(customtitle);
        showLoading();
        $.get(Url, function (data) {
            if (data.isSuccessful === false) {

                showToastr('warning', data.message);
            } else {

                $('#form-modal-place .modal-body').html(data);
                initializingAutocomplete();
                $.validator.unobtrusive.parse($('#form-modal-place form'));
                initializeTinyMCE();
                activatingInputAttributes();
                initializeSelect2();
                if (typeof actionsAfterLoadModalForm === 'function') {
                    actionsAfterLoadModalForm();
                }
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




// فرم ایجاد و ویرایش در داخل مودال موقعی که سابمیت شوند توسط این
// فانکشن به صورت ایجکسی به سمت سرور ارسال میشوند

$(document).on('submit', 'form.custom-ajax-form', function (e) {

    e.preventDefault();
    var currentForm = $(this);
    var closewhedone = currentForm.attr('close-when-done');
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
        success: function (data) {

            if (data.isSuccessful == false) {
                fillValidationForm(data.data, currentForm);
                showToastr('warning', data.message);
            } else {
                fillDataTable();
                if (closewhedone !== 'false') {
                    $("#form-modal-place").modal("hide");
                }
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

// فعال ساز مربوط به صحفه بندی
function activatingPagination() {
    $("#main-pagianation button").click(function () {
        isMainPaginationClicked = true;
        var currentPageSelected = $(this).val();
        $('.Search-form-via-ajax input[name$="Pagination.CurrentPage"]').val(currentPageSelected);
        $('.Search-form-via-ajax').submit();
    });
}

// فعال ساز دکمه برو به صفحه فلان
function activatingGotoPage() {
    $("#goto-page-button").click(function () {
        isGotoPageClicked = true;

    });
}


// خواندن اطلاعات و ریختن آن در داخل گرید
function fillDataTable() {
    $('.read-data-table .data-table-body').remove();
    $('#record-not-found-box').remove();
    $('.search-form-loading').attr('disabled', 'disabled');
    $('.data-table-loading').removeClass('d-none');

    var currentForm = $('form.Search-form-via-ajax');

    const formData = currentForm.serializeArray();

    $.get(`${location.pathname}?handler=GetDataTable`, formData, function(data, status) {
            if (data.isSuccessful === false) {
                fillValidationForm(data.data, currentForm);
                showToastr('warning', data.message);
            } else {
                $('.read-data-table').append(data);
                activatingPagination();
                activatingGotoPage();
                activatingModalForm();
                activatingPageCount();
                activatingDeleteButtons();
                enablingTooltips();
                activatingGetHtmlWithAjax();
            }
        }).fail(function() {
        ShowErrorMessage();
    }).always(function() {
        $('.Search-form-loading').removeAttr('disabled');
        $('.data-table-loading').addClass('d-none');
    });
}

// فعالساز مربوط به تعداد آیتم در هر صفحه
function activatingPageCount() {

    $('#page-count-selectbox').change(function () {

        var pageCountValue = this.value;
        $('.Search-form-via-ajax input[name$="Pagination.PageCount"]').val(pageCountValue);
        $('.Search-form-via-ajax').submit();
    });
}

// برای مثال در صفحه دو یک گرید هستیم
// و کاربر یک عبارتی را سرچ میکند ما باید بیاییم
// و از صفحه یک دوباره شروع به نمایش دادن اطلاعات کنیم
// این متغیر برای این کار است
var isMainPaginationClicked = false;

// اگر دکمه برو به فلان صحفحه کلیک شده بود
// باید به همان صفحه برویم
var isGotoPageClicked = false;
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
                activatingGetHtmlWithAjax();
            }

        } else {
            ShowErrorMessage();
        }
    });

});


// موقعی که یک فرم به سمت سرور ارسال میشود
// اگر خطای اعتبار سنجی داشته باشد
// با استفاده از این فانکشن متن خطاها را داخل
// <div asp-validation-summary="All" class="text-danger"></div>
// نمایش میدهیم
function fillValidationForm(errors, currentForm) {
    var result = '<ul>';
    errors.forEach(function (e) {
        result += `<li>${e}</li>`;
    });
    result += '</ul>';
    currentForm.find('div[class*="validation-summary"]').html(result);

}




// این فانکشن هر فرمی را به صورت پست به سمت سرور با استفاده از ایجکس
// ارسال میکند
$(document).on('submit', 'form.public-ajax-form', function (e) {

    e.preventDefault();
    var currentForm = $(this);
    var formAction = currentForm.attr("action");
    var functionName = currentForm.attr("call-function-in-the-end");
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
            showLoading();
        },
        success: function (data, status) {

            if (data.isSuccessful == false) {
                /*var finalData = data.data != null ? data.data : [data.message];*/
                var finalData = data.data || [data.message];
                fillValidationForm(finalData, currentForm);
                showToastr('warning', data.message);
            } else {
                window[functionName](data.message, data.data);
            }
        },
        complete: function () {
            hideLoading();
        },
        error: function () {
            ShowErrorMessage();
        }

    });
});

// با استفاده از این فانکشن میتوانیم اطلاعاتی را از سمت سرور دریافت کنیم
// برای مثال برای خواندن شهرستان های یک استان از این فانکشن استفاده میکنیم
function getDateWithAjax(url, formdata, functionNameToCallInTheEnd) {
    $.ajax({
        url: url,
        data: formdata,
        type: 'Get',
        dataType: 'json',
        beforeSend: function () {
            showLoading();
        },
        success: function (data, status) {

            if (data.isSuccessful === false) {
                showToastr('warning', data.message);
            } else {
                window[functionNameToCallInTheEnd](data.message, data.data);
            }
        },
        complete: function () {
            hideLoading();
        },
        error: function () {
            ShowErrorMessage();
        }

    });
}

//End ajax Operation
// به محض بلر شدن یک اینپوت
// تمامی اینپوت های فرم را مجددا اعتبار سنجی میکند
// چرا از این استفاده میکنیم ؟
// برای مثال شما روی دکمه ثبت نام کلیک میکنید و
// در بالای صفحه و داخل تگ
// <div asp-validation-summary="All" class="text-danger"></div>
// مینویسد ایمیل را وارد کنید
// شما نیز ایمیل را وارد میکنید
// اما در قسمت بالای صفحه همچنان متن "لطفا ایمیل را وارد کنید" وجود دارد
// برای اینکه این مشکل حل شود از این کد استفاده میکنیم
$('form input').blur(function () {
    $(this).parents('form').valid();
});

$('form input.custom-md-persian-datepicker').change(function () {
    $(this).parents('form').valid();
});

$('form select').change(function () {
    $(this).parents('form').valid();
});

$('form input[type="checkbox"] ,input[type="file"] ').change(function () {
    $(this).parents('form').valid();
});


function activatingGetHtmlWithAjax() {

    $('.get-html-with-ajax').click(function () {

        var funcToCall = $(this).attr('functionNameToCallOnClick');
        window[funcToCall](this);
    });
}

// خواندن صفحات
// html
// از سمت سرور
function GetHtmlWithAjax(url, data, functionNameToCallInTheEnd, clickedButton) {
    $.ajax({
        url: url,
        data: data,
        type: 'GET',
        dataType: 'html',
        traditional: true,
        beforeSend: function () {
            showLoading();
        },
        success: function (data) {
            if (data.isSuccessful === false) {
                showToastr('warning', data.message);
            } else {
                window[functionNameToCallInTheEnd](data, clickedButton);
            }
        },
        complete: function () {
            hideLoading();
        },
        error: function () {
            ShowErrorMessage();
        }
    });
}

function activatingInputAttributes() {
    $('input[data-val-ltrdir="true"]').attr('dir', 'ltr');
    $('input[data-val-isimage]').attr('accept', 'image/*');
}


// نمایش پیش نمایش عکس
$('.image-preivew-input').change(function() {
    var selectedFile = this.files[0];
    var imagePreviewBox = $(this).attr('image-preview-box');
    if (selectedFile && selectedFile.size > 0) {
        $(`#${imagePreviewBox}`).removeClass('d-none');
        $(`#${imagePreviewBox} img`).attr('src', URL.createObjectURL(selectedFile));
    } else {
        $(`#${imagePreviewBox} img`).attr('src', '');
        $(`#${imagePreviewBox}`).addClass('d-none');
    }
});


$(function() {
    activatingInputAttributes();
    initializeSelect2WithoutModal();
    initializeTinyMCE();
    enablingTooltips();
    $('textarea[add-image-plugin="true"]').each(function() {

        var elementId = $(this).attr('id');
        var currentTinyMce = tinymce.get(elementId);
        currentTinyMce.settings.plugins += ' image';
        currentTinyMce.settings.toolbar[4].items.push('image');
        currentTinyMce.settings.image_title = true;
    });

    $('textarea.custom-tinymce').each(function () {
        var elementId = $(this).attr('id');
        var uploadimageurl = $(this).attr("upload-image-url");
        var TinyMceInc = tinymce.get(elementId);
        TinyMceInc.settings.images_upload_handler = function (blobInfo, success, failure, progress) {

            sendTinyMceImagesToServer(blobInfo, success, failure, progress, uploadimageurl);

        };
    });

});