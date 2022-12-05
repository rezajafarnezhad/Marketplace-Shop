
var IsAuthenticated;
var LoginPageUrl;


$(document).ready(function () {


    var win = $(this);
    var mainInputEl = $('#main-search-input');
    if (win.width() > 456) {
        mainInputEl.attr('placeholder', 'نام کالا، برند یا دسته مورد نظر خود را جستجو نمایید ...');
    } else {
        mainInputEl.attr('placeholder', 'جستجو ...');
    }
    $(window).on('resize', function () {
        win = $(this);
        if (win.width() > 456) {
            mainInputEl.attr('placeholder', 'نام کالا، برند یا دسته مورد نظر خود را جستجو نمایید ...');
        } else {
            mainInputEl.attr('placeholder', 'جستجو ...');
        }
    });


    IsAuthenticated = $('body').attr('is-authenticated') === 'true';
    LoginPageUrl = $('body').attr('login-page-url');

    $('body').removeattr('is-authenticated')
    $('body').removeattr('login-page-url')

});


const firstLoginModalBody = `<div class="modal" id="first-login-modal">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-body text-center">
                <h5>
                    لطفا ابتدا وارد حساب خود شوید
                </h5>
                <a class="btn btn-secondary">ورود</a>
                <button type="button" class="btn btn-danger" data-bs-dismiss="modal">بستن</button>
            </div>
        </div>
    </div>
</div>`;


function showFirstLoginModal() {

    if ($('#first-login-modal').length === 0) {
        $('body').append(firstLoginModalBody);
        $('#first-login-modal a').attr('href', LoginPageUrl);
    }

    $('#first-login-modal').modal('show');


}

