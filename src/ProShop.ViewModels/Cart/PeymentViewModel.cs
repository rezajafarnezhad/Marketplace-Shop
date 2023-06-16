
using ProShop.Entities;

namespace ProShop.ViewModels.Cart
{

    public class PeymentViewModel
    {
        public List<ShowCartInPeymentPageViewModel> CartItems { get; set; }
    }

    public class ShowCartInPeymentPageViewModel : ShowCartInCartPageViewModel
    {
        public ProductDimensions ProductVariantProductDimensions { get; set; }
    }


    public class CreateOrderAndPayViewModel
    {
        /// <summary>
        /// آیا این سفارش توسط مقدار داخل کیف پول کاربر پرداخت شده است ؟
        /// اگر فالس باشد یعنی توسط درگاه اینترنتی پرداخت شده است
        /// اگر هم ترو باشد، یعنی توسط کیف پول پرداخت شده است
        /// </summary>
        public bool PayFromWallet { get; set; }
    }

    public class ShowCartForCreateOrderAndPayViewModel : ShowCartInPeymentPageViewModel
    {
        public long ProductVariantGaranteeId{ get; set; }

    }

}

