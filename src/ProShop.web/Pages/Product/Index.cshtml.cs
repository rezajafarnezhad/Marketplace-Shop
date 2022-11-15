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
    private readonly IUnitOfWork unitOfWork;
    public IndexModel(IProductService productService, IUserProductFavoriteService userFavoriteService, IUnitOfWork unitOfWork)
    {
        _productService = productService;
        _userFavoriteService = userFavoriteService;
        this.unitOfWork = unitOfWork;
    }

    public ShowProductInfoViewModel ProductInfo { get; set; }

    public async Task<IActionResult> OnGet(int productCode, string slug)
    {

        ProductInfo = await _productService.GetProductInfo(productCode);
        if (ProductInfo is null)
            return RedirectToPage(PublicConstantStrings.Error404PageName);

        if (ProductInfo.Slug != slug)
            return RedirectToPage("Index", new { productCode, slug = ProductInfo.Slug });


        return Page();

    }
    public async Task<IActionResult> OnPostAddOrRemoveFavorite(long Id, bool addFavorite)
    {
        if (!User.Identity.IsAuthenticated)
            return Json(new JsonResultOperation(false));

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
}
