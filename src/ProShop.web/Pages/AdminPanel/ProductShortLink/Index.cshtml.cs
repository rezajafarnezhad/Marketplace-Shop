using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.Services.Implements;
using ProShop.ViewModels.ProductShortLink;

namespace ProShop.web.Pages.AdminPanel.ProductShortLink;


[CheckModelStateInRazorPages]
public class IndexModel : PageBase
{
    private readonly IProductShortLinkService _productShortLinkService;

    public IndexModel(IProductShortLinkService productShortLinkService)
    {
        _productShortLinkService = productShortLinkService;
    }

    [BindProperty(SupportsGet = true)]
    public ShowProductShortLinksViewModel ShortLink { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetGetDataTableAsync()
    {

        return Partial("_List", await _productShortLinkService.GetAllProductShowLink(ShortLink));
    }
}
