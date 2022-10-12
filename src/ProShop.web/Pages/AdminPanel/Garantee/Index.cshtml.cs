using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.Services.Implements;
using ProShop.ViewModels.Garantee;

namespace ProShop.web.Pages.AdminPanel.Garantee;

public class IndexModel : PageBase
{

    private readonly IGaranteeService _garanteeService;

    public IndexModel(IGaranteeService garanteeService)
    {
        _garanteeService = garanteeService;
    }

    [BindProperty(SupportsGet =true)]
    public ShowGarantiesViewModel Garanties { get; set; } = new();
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

        return Partial("_List", await _garanteeService.GetGaranties(Garanties));
    }
}
