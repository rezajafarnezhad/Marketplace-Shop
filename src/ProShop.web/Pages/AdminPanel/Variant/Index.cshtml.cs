using AutoMapper;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Implements;
using ProShop.ViewModels.Brands;
using ProShop.ViewModels.Veriants;

namespace ProShop.web.Pages.AdminPanel.Variant;

public class IndexModel : PageBase
{
    private readonly IVariantService _variantService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    [BindProperty(SupportsGet = true)]
    public ShowVeriantsViewModel Variants { get; set; } = new();
    
    public IndexModel(IVariantService variantService, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _variantService = variantService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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

    public async Task<IActionResult> OnGetAdd()
    {
        return Partial("Add");
    }
    
    public async Task<IActionResult> OnPostAdd(AddVariantInPanelAdmin model)
    {
        if(!ModelState.IsValid)
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });

        var _variant = _mapper.Map<Entities.Variant>(model);
        _variant.IsConfirmed=true;
        var result = await _variantService.AddAsync(_variant);
        if (!result.Ok)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.DuplicateErrorMessage)
            {
                Data = result.Columns.SetDuplicateColumnsErrors<AddVariantInPanelAdmin>()
            });
        }

        await _unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "تنوع مورد نظر با موفقیت ذخیره شد"));

    }

}
