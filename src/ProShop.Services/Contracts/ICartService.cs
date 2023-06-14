using ProShop.Entities;
using ProShop.ViewModels.Cart;
using ProShop.ViewModels.Product;

namespace ProShop.Services.Contracts;

public interface ICartService : IGenericService<Cart>
{
    Task<List<Cart>> GetAllCartItems(long userId);
    Task<List<ShowCartInCartPageViewModel>> GetCartForCartPage(long userId);
    Task<List<ShowCartInDropDownViewModel>> GetCartForDropDown(long userId);
    Task<List<ShowCartInChackoutPageViewModel>> GetCartsForCheckoutPage(long userId);
    Task<List<ShowCartInPeymentPageViewModel>> GetCartsForPeymentPage(long userId);
    Task<List<ProductVariantInCartForProductInfoViewModel>> GetProductVariantsInCart(List<long> productVariantsIds, long userId);
}
