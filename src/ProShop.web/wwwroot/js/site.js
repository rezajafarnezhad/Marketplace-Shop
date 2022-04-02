
var rvt = '__RequestVerificationToken';

var loadingModalHtml = `<div class="modal fade" id="loading-modal" data-bs-backdrop="static">
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
function showLoading(funcToCall) {
    if ($('#loading-modal').length === 0) {
        $('body').append(loadingModalHtml);
    }
    $('#loading-modal').on('shown.bs.modal', function (e) {
        funcToCall();
    })
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