using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Services.Implements;
using ProShop.ViewModels.Garantee;

namespace ProShop.web.Pages.AdminPanel.Garantee;

public class IndexModel : PageBase
{

    private readonly IGaranteeService _garanteeService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly IUploadFileService _uploadFileService;
    public IndexModel(IGaranteeService garanteeService, IMapper mapper, IUnitOfWork unitOfWork, IUploadFileService uploadFileService)
    {
        _garanteeService = garanteeService;
        _mapper = mapper;
        this.unitOfWork = unitOfWork;
        _uploadFileService = uploadFileService;
    }

    [BindProperty(SupportsGet = true)]
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

    public IActionResult OnGetAdd()
    {
        return Partial("Add");
    }

    public async Task<IActionResult> OnPostAdd(AddGarantee garantee)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });

        }

        string GaranteePicture = garantee.Picture.GenerateFileName();
        var _grantee = _mapper.Map<Entities.Garantee>(garantee);
        _grantee.Picture = GaranteePicture;
        _grantee.IsConfirmed = true;
        await _garanteeService.AddAsync(_grantee);
        await unitOfWork.SaveChangesAsync();
        await _uploadFileService.SaveFile(garantee.Picture, GaranteePicture, null, "images", "Garantee");
        return Json(new JsonResultOperation(true, "گارانتی با موفقیت ثبت شد"));
    }
}
