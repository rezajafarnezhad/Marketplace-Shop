using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.IdentityToolkit;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Cart;

namespace ProShop.web.Pages.Carts;


[Authorize]
public class CheckOutModel : PageModel
{
    private readonly IAddressService _addressService;
    private readonly ICartService _cartService;

    public CheckOutModel(ICartService cartService, IAddressService addressService)
    {
        _cartService = cartService;
        _addressService = addressService;
    }

    public CheckoutViewModel CheckOutPage { get; set; } = new CheckoutViewModel();

    public async Task<IActionResult> OnGet()
    {
        var userId = User.Identity.GetLoggedUserId();
        CheckOutPage.CartItems = await _cartService.GetCartsForCheckoutPage(userId);
        if(CheckOutPage.CartItems.Count < 1)
        {
            return RedirectToPage("Index");
        }
        CheckOutPage.UserAddress = await _addressService.GetAddressForCheckoutPage(userId);
        return Page();
    }
}
