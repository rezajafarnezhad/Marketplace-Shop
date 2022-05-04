using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.ViewModels.Brands;

namespace ProShop.web.Pages.AdminPanel.Brand;

public class IndexModel : PageBase
{

    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadFileService _fileService;
    public IndexModel(IBrandService brandService, IMapper mapper, IUnitOfWork unitOfWork, IUploadFileService fileService)
    {
        _brandService = brandService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    [BindProperty(SupportsGet = true)]
    public ShowBrandsViewModel ShowBrands { get; set; } = new();
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetGetDataTableAsync()
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(String.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        return Partial("_List", await _brandService.GetBrands(ShowBrands));
    }


    public IActionResult OnGetAdd()
    {
        return Partial("Add");
    }

    public async Task<IActionResult> OnPostCheckForTitleFa(string titlefa)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleFa), titlefa));

    }
    public async Task<IActionResult> OnPostCheckForTitleEn(string titleEn)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleEn), titleEn));
    }

    public async Task<IActionResult> OnPostAdd(AddBrandViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var BrandLogoName = model.LogoPicture.GenerateFileName();
        var _Brand = _mapper.Map<Entities.Brand>(model);
        _Brand.LogoPicture = BrandLogoName;
        _Brand.IsConfirmed = true;
        string brandRegistrationPicture = null;
        if (model.BrandRegistrationPicture.IsFileUploaded())
            brandRegistrationPicture = model.BrandRegistrationPicture.GenerateFileName();

        _Brand.BrandRegistrationPicture = brandRegistrationPicture;

        var result = await _brandService.AddAsync(_Brand);
        if (!result.Ok)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.DuplicateErrorMessage)
            {
                Data = result.Columns.SetDuplicateColumnsErrors<AddBrandViewModel>()
            });
        }

        await _unitOfWork.SaveChangesAsync();

        await _fileService.SaveFile(model.LogoPicture, BrandLogoName, null, "images", "brands");

        if (brandRegistrationPicture != null)
            await _fileService.SaveFile(model.BrandRegistrationPicture, brandRegistrationPicture, null, "images", "BrandRegistrationPictures");


        return Json(new JsonResultOperation(true, "برند مورد نظر با موفقیت ذخیره شد"));
    }


    public async Task<IActionResult> OnGetEdit(long Id)
    {
        if (Id < 1)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var model = await _brandService.GetForEdit(Id);
        if (model is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("Edit", model);
    }

    public async Task<IActionResult> OnPostCheckForEditTitleFa(string titlefa, long Id)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleFa), titlefa, Id));

    }
    public async Task<IActionResult> OnPostCheckForEditTitleEn(string titleEn, long Id)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleEn), titleEn, Id));
    }


    public async Task<IActionResult> OnPostEdit(EditBrandViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        string LogoFileName = null;
        if (model.NewLogoPicture.IsFileUploaded())
            LogoFileName = model.NewLogoPicture.GenerateFileName();

        string BrandRegistrationPicture = null;
        if (model.NewBrandRegistrationPicture.IsFileUploaded())
            BrandRegistrationPicture = model.NewBrandRegistrationPicture.GenerateFileName();

        var brand = await _brandService.FindByIdAsync(model.Id);
        var oldLogoFileName = brand.LogoPicture;
        var oldBrandRegistrationPictureFileName = brand.BrandRegistrationPicture;

        var _brand = _mapper.Map(model,brand);
        
        if (LogoFileName is not null)
        {
            _brand.LogoPicture = LogoFileName;
            await _fileService.SaveFile(model.NewLogoPicture, LogoFileName, oldLogoFileName, "images", "brands");
        }
        else
        {
            _brand.LogoPicture = oldLogoFileName;

        }
        if (BrandRegistrationPicture is not null)
        {
            _brand.BrandRegistrationPicture = BrandRegistrationPicture;
            await _fileService.SaveFile(model.NewBrandRegistrationPicture, BrandRegistrationPicture, oldBrandRegistrationPictureFileName, "images", "BrandRegistrationPictures");
        }
        else
        {
            _brand.BrandRegistrationPicture = oldBrandRegistrationPictureFileName;
        }

        var result = await _brandService.Update(_brand);
        if (!result.Ok)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.DuplicateErrorMessage)
            {
                Data = result.Columns.SetDuplicateColumnsErrors<EditBrandViewModel>()
            });
        }

        await _unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "برند مورد نظر با موفقیت ویرایش شد"));
    }

    public async Task<IActionResult> OnGetBrnadDetails(long brandId)
    {
        var model = await _brandService.GetBrandDetails(brandId);
        if (model is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("BrnadDetails",model);
    }

    public async Task<IActionResult> OnPostRejectBrand(BrandDetailsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, "لطفا دلیل رد برند را وارد کنید"));
        }

        var _brand = await _brandService.GetInActiveBrand(model.Id);
        if (_brand == null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var LogoFilename = _brand.LogoPicture;
        var BrandRegistrationPictureFilename = _brand.BrandRegistrationPicture;

        _brandService.Remove(_brand);
        await _unitOfWork.SaveChangesAsync();

        //Delete Pictures
         _fileService.DeleteFile(LogoFilename, "images", "brands");
         _fileService.DeleteFile(BrandRegistrationPictureFilename, "images", "BrandRegistrationPictures");

        //ToDO: Send Email

        return Json(new JsonResultOperation(true, "برند مورد نظر با موفقیت رد شد"));
    }
    public async Task<IActionResult> OnPostConfirmBrand(long Id)
    {
        if (Id < 1)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var _brand = await _brandService.GetInActiveBrand(Id);
        if (_brand == null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        _brand.IsConfirmed = true;
        await _unitOfWork.SaveChangesAsync();

        //Send Email 

        return Json(new JsonResultOperation(true, "برند مورد نظر با موفقیت تایید شد"));
    }
}