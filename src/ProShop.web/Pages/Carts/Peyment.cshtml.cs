using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.IdentityToolkit;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Cart;

namespace ProShop.web.Pages.Carts
{
    [Authorize]
    public class PeymentModel : PageModel
    {
        private readonly ICartService _cartService;

        public PeymentModel(ICartService cartService)
        {
            _cartService = cartService;
        }

        public PeymentViewModel PeymentPage { get; set; } = new PeymentViewModel();

        public async Task<IActionResult> OnGet()
        {
            var userId = User.Identity.GetLoggedUserId();
            PeymentPage.CartItems = await _cartService.GetCartsForPeymentPage(userId);
            if (PeymentPage.CartItems.Count < 1)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
