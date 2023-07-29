using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProShop.Common.Constants;
using ProShop.DataLayer.Migrations;
using ProShop.Services.Contracts;
using System.Text.RegularExpressions;

namespace ProShop.web.Pages.Product;

public class ProductShortLinkModel : PageModel
{
    private readonly IProductService _productService;

    public ProductShortLinkModel(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> OnGet(string productshortLink)
    {
        if (!Regex.IsMatch(productshortLink, @"^[A-Z0-9]{1,10}$"))
        {
            return RedirectToPage(PublicConstantStrings.Error404PageName);
        }

        var product = await _productService.FindByShortLink(productshortLink);
        if (product.slug is null)
            return RedirectToPage(PublicConstantStrings.Error404PageName);


        
      
        return RedirectToPagePermanent("/Product/Index", new
        {
            product.slug,
            product.productCode
        });
    }
}
