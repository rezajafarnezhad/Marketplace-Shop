using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.Entities;
using ProShop.ViewModels.Veriants;

namespace ProShop.web.Pages.AdminPanel.Variant;

public class IndexModel : PageBase
{
    private readonly IVariantService _variantService;

    [BindProperty(SupportsGet = true)]
    public ShowVeriantsViewModel Variants { get; set; } = new();
    
    public IndexModel(IVariantService variantService)
    {
        _variantService = variantService;
    }

    public void OnGet()
    {

    }


    public async Task<IActionResult> OnGetGetDataTableAsync()
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        return Partial("_List", await _variantService.GetVariants(Variants));
    }

}
