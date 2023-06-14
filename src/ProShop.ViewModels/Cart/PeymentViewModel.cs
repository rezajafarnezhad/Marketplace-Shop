
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

}

