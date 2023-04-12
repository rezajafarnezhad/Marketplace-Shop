using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Cart;

namespace ProShop.web.Pages.Carts;

public class IndexModel : PageBase
{

    private readonly ICartService _cartService;
    private readonly IUnitOfWork unitOfWork;
    private readonly IProductVariantService _productVariantService;
    private readonly IViewRenderService _viewRendererService;

    public IndexModel(ICartService cartService, IUnitOfWork unitOfWork, IProductVariantService productVariantService = null, IViewRenderService viewRendererService = null)
    {
        _cartService = cartService;
        this.unitOfWork = unitOfWork;
        _productVariantService = productVariantService;
        _viewRendererService = viewRendererService;
    }

    public List<ShowCartInCartPageViewModel> CartItems { get; set; }

    public async Task OnGet()
    {
        var userId = User.Identity.GetLoggedUserId();
        CartItems = await _cartService.GetCartForCartPage(userId);
    }

    public async Task<IActionResult> OnPostAddProductVariantToCart(long productVariantId, bool isIncrease)
    {
        var ProductVariant = await _productVariantService.FindByIdAsync(productVariantId);
        if (ProductVariant is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var userId = User.Identity.GetLoggedUserId();

        var cart = await _cartService.FindAsync(userId, productVariantId);
        if (cart is null)
        {
            var CartToAdd = new Cart()
            {
                ProductVaraintId = productVariantId,
                UserId = userId,
                Count = 1
            };


            await _cartService.AddAsync(CartToAdd);
        }
        else if (isIncrease)
        {
            cart.Count++;
            if (cart.Count > ProductVariant.MaxCountInCart)
                cart.Count = ProductVariant.MaxCountInCart;

            //ProductVariant.Count موجودی انبار
            if (cart.Count > ProductVariant.Count)
                cart.Count = (short)ProductVariant.Count;
        }
        else
        {
            cart.Count--;
            if (cart.Count == 0)
                _cartService.Remove(cart);
        }




        await unitOfWork.SaveChangesAsync();

        var carts = await _cartService.GetCartForCartPage(userId);

        var cartbody = string.Empty;
        if (carts.Count == 0)
        {
            cartbody = await _viewRendererService.RenderViewToStringAsync("~/Pages/Carts/_EmptyCart.cshtml");

        }
        else
        {
            cartbody = await _viewRendererService.RenderViewToStringAsync("~/Pages/Carts/_CartBody.cshtml", carts);

        }



        return Json(new JsonResultOperation(true, string.Empty)
        {

            Data = new
            {
                cartbody = cartbody
            }

        });

    }

    public async Task<IActionResult> OnPostRemoveAllItemsInCart()
    {
        var userid = User.Identity.GetLoggedUserId();
        var allItemsInCart = await _cartService.GetAllCartItems(userid);
        _cartService.RemoveRange(allItemsInCart);
        await unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, String.Empty)
        {
            Data = new
            {
                cartbody = await _viewRendererService.RenderViewToStringAsync("~/Pages/Carts/_EmptyCart.cshtml")

            }
        });
    }
}
