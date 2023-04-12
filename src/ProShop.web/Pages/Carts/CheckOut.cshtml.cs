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
    private readonly ICartService _cartService;

    public CheckOutModel(ICartService cartService)
    {
        _cartService = cartService;
    }

    public List<ShowCartInChackoutPage> CartItems { get; set; }

    public async Task OnGet()
    {
        var userId = User.Identity.GetLoggedUserId();
        CartItems = await _cartService.GetCartsForCheckoutPage(userId);
    }
}
