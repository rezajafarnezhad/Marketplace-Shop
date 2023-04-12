using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Product;
using System.Security.AccessControl;

namespace ProShop.web.Pages.Product;

public class IndexModel : PageBase
{

    private readonly IProductService _productService;
    private readonly IUserProductFavoriteService _userFavoriteService;
    private readonly IProductVariantService _productVariantService;
    private readonly ICartService _cartService;
    private readonly IUnitOfWork unitOfWork;
    private readonly IViewRenderService _viewRendererService;

    public IndexModel(IProductService productService, IUserProductFavoriteService userFavoriteService, IUnitOfWork unitOfWork, IProductVariantService productVariantService, ICartService cartService, IViewRenderService viewRendererService)
    {
        _productService = productService;
        _userFavoriteService = userFavoriteService;
        this.unitOfWork = unitOfWork;
        _productVariantService = productVariantService;
        _cartService = cartService;
        _viewRendererService = viewRendererService;
    }

    public ShowProductInfoViewModel ProductInfo { get; set; }

    public async Task<IActionResult> OnGet(int productCode, string slug)
    {

        ProductInfo = await _productService.GetProductInfo(productCode);
        if (ProductInfo is null)
            return RedirectToPage(PublicConstantStrings.Error404PageName);

        if (ProductInfo.Slug != slug)
            return RedirectToPage("Index", new { productCode, slug = ProductInfo.Slug });

        var userid = User.Identity.GetLoggedUserId();
        var ProductVariantsIds = ProductInfo.ProductVariants.Select(c => c.Id).ToList();
        ProductInfo.ProductVariantInCart = await _cartService.GetProductVariantsInCart(ProductVariantsIds, userid);

        return Page();

    }
    public async Task<IActionResult> OnPostAddOrRemoveFavorite(long Id, bool addFavorite)
    {
        if (!User.Identity.IsAuthenticated)
            return Json(new JsonResultOperation(false, "ابتدا به حساب کاربری خود وارد شوید"));

        if (!await _productService.IsExistsBy(nameof(Entities.Product.Id), Id))
            return Json(new JsonResultOperation(false));


        var userId = User.Identity.GetLoggedUserId();
        var userProductFavorite = await _userFavoriteService.FindAsync(userId, Id);
        if (userProductFavorite is null && addFavorite)
        {
            await _userFavoriteService.AddAsync(new UserProductFavorite()
            {
                ProductId = Id,
                UserId = userId
            });
        }
        else if (userProductFavorite != null && !addFavorite)
        {
            _userFavoriteService.Remove(userProductFavorite);
        }

        await unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true));
    }

    //Id is ProductVariantId
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
            if(cart.Count > ProductVariant.Count)
                cart.Count = (short)ProductVariant.Count;
        }
        else
        {
            cart.Count--;
            if (cart.Count == 0)
                _cartService.Remove(cart);
        }




        await unitOfWork.SaveChangesAsync();


        // اگر کاونت سبد خرید برابر با مکس تعیین شده توسط فروشنده بود
        // یا مساوی تعداد موجودی داخل انبار، این متغیر ترو میشود

        var IsCartFull = ProductVariant.MaxCountInCart == (cart?.Count ?? 1) || (cart?.Count ?? 1) == ProductVariant.Count;


        var carts = await _cartService.GetCartForDropDown(userId);

        return Json(new JsonResultOperation(true, "محصول مورد نظر به ثبت خرید شما اضافه شد")
        {

            Data = new
            {
                count = cart?.Count ?? 1,
                productvariantid = productVariantId,
                iscartfull = IsCartFull,
                cartsDetails = await _viewRendererService.RenderViewToStringAsync("~/Pages/Shared/_Cart.cshtml",carts)
            }

        });

    }

}
