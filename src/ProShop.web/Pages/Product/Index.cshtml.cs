using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Constants;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Product;

namespace ProShop.web.Pages.Product;

public class IndexModel : PageModel
{

    private readonly IProductService _productService;

    public IndexModel(IProductService productService)
    {
        _productService = productService;
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
}
