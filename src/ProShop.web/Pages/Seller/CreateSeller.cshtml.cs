using System.Drawing.Printing;
using AngleSharp.Css.Dom;
using AutoMapper;
using DNTPersianUtils.Core;
using DNTPersianUtils.Core.IranCities;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Core.Types;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;
using ProShop.Services.Contracts.Identity;
using ProShop.Services.Implements.Identity;
using ProShop.ViewModels.Sellers;

namespace ProShop.web.Pages.Seller;

public class CreateSellerModel : PageBase
{
    private readonly IApplicationUserManager _userManager;
    private readonly IProvinceAndCityService _provinceAndCityService;
    private readonly ISellerService _sellerService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadFileService _fileService;
    private readonly IApplicationSigninManager _applicationSigninManager;
    private readonly IHtmlSanitizer _htmlSanitizer;
    public CreateSellerModel(IApplicationUserManager userManager, IProvinceAndCityService provinceAndCityService, ISellerService sellerService, IMapper mapper, IUnitOfWork unitOfWork, IUploadFileService fileService, IApplicationSigninManager applicationSigninManager, IHtmlSanitizer htmlSanitizer)
    {
        _userManager = userManager;
        _provinceAndCityService = provinceAndCityService;
        _sellerService = sellerService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _applicationSigninManager = applicationSigninManager;
        _htmlSanitizer = htmlSanitizer;
    }


    [BindProperty]
    public CreateSellerViewModel CreateSeller { get; set; } = new();
    public async Task<IActionResult> OnGet(string phoneNumber)
    {
        
        if (!await _userManager.CheckForUserIsSeller(phoneNumber))
        {
            return RedirectToPage("/Error");
        }

        CreateSeller.PhoneNumber = phoneNumber;
        CreateSeller = await _userManager.GetUserInfoForCreateSeller(phoneNumber);
        var Provinces = await _provinceAndCityService.GetProvincesToShowSelectBox();
        CreateSeller.Provinces = Provinces.CreateSelectListItem();
        return Page();

    }

    public async Task<IActionResult> OnGetGetCities(long ProvinceId)
    {
        if (ProvinceId == 0)
            return Json(new JsonResultOperation(true, string.Empty)
            {
                Data = new Dictionary<long, string>()
            });


        if (ProvinceId < 1)
            return Json(new JsonResultOperation(false, "شهرستان مورد نظر یافت نشد"));


        if (!await _provinceAndCityService.IsExistsBy(nameof(Entities.ProvinceAndCity.Id), ProvinceId))
            return Json(new JsonResultOperation(false, "شهرستان مورد نظر یافت نشد"));


        var _cities = await _provinceAndCityService.GetCitiesByProvinceIdInSelectBox(ProvinceId);
        return Json(new JsonResultOperation(true, string.Empty)
        {
            Data = _cities
        });
    }

    public async Task<IActionResult> OnGetCheckForShopName(CreateSellerViewModel CreateSeller)
    {
        return Json(!await _sellerService.IsExistsBy(nameof(Entities.Seller.ShopName), CreateSeller.ShopName));
    }
    
    public async Task<IActionResult> OnGetCheckForShabaNumber(CreateSellerViewModel CreateSeller)
    {
        return Json(!await _sellerService.IsExistsBy(nameof(Entities.Seller.ShabaNumber), CreateSeller.ShabaNumber));
    }


    public async Task<IActionResult> OnPost()
    {

        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }


        bool isReal;

        if (!CreateSeller.IsLegalPerson)
        {
            CreateSeller.CompanyName
                = CreateSeller.RegisterNumber
                    = CreateSeller.EconomicCode
                        = CreateSeller.SignatureOwners
                            = CreateSeller.NationalId
                                = null;
            CreateSeller.CompanyType = null;
            isReal = true;
        }
        else
        {
            isReal = false;
            var legalPersonProperties = new List<string>()
            {
                nameof(CreateSeller.CompanyName),
                nameof(CreateSeller.RegisterNumber),
                nameof(CreateSeller.EconomicCode),
                nameof(CreateSeller.SignatureOwners),
                nameof(CreateSeller.NationalId)
            };
            ModelState.CheckStringInputs(legalPersonProperties, CreateSeller);
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }
        }



        var _user = await _userManager.GetUserForCreateSeller(CreateSeller.PhoneNumber);

        if (_user is null)
            return Json(new JsonResultOperation(false, "کاربر مورد نظر یافت نشد"));

        _user = _mapper.Map(CreateSeller, _user);

        var birthDateResult = CreateSeller.BirthDate.ToGregorianDateForCreateSeller();

        if (!birthDateResult.IsOK)
            return Json(new JsonResultOperation(false, "تاریخ تولد به درستی وارد کنید"));

        if (!birthDateResult.IsRangeOk)
            return Json(new JsonResultOperation(false, "سن باید بین ۱۸ و ۱۰۰ سال باشد"));

        _user.BirthDate = birthDateResult.ConvertedDate;
        if (CreateSeller.AboutSeller != null)
        {
            CreateSeller.AboutSeller = _htmlSanitizer.Sanitize(CreateSeller.AboutSeller);
        }

        var _seller = _mapper.Map<Entities.Seller>(CreateSeller);
        _seller.IsRealPerson = isReal;
        _seller.IsActive = true;
        _seller.UserId = _user.Id;
        _seller.SellerCode = await _sellerService.GetSellerCodeForCreateSeller();

        string IdCartPictureName = CreateSeller.IdCartPictureFile.GenerateFileName();

        string LogoName = null;
        if (CreateSeller.LogoFile.IsFileUploaded())
            LogoName = CreateSeller.LogoFile.GenerateFileName();

        _seller.IdCartPicture = IdCartPictureName;
        _seller.Logo = LogoName;


        var result = await _sellerService.AddAsync(_seller);
        if (!result.Ok)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.DuplicateErrorMessage)
            {
                Data = result.Columns.SetDuplicateColumnsErrors<CreateSellerViewModel>()
            });
        }


        //Add Role Seller 
        var roleResult = await _userManager.AddToRoleAsync(_user, ConstantRoles.Seller);
        if (!roleResult.Succeeded)
        {
            ModelState.AddErrorsFromResult(roleResult);
            return Json(new JsonResultOperation(false)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        await _unitOfWork.SaveChangesAsync();

        await _fileService.SaveFile(CreateSeller.IdCartPictureFile, IdCartPictureName, null, "images", "seller-id-cart-pictures");

        if (LogoName != null)
            await _fileService.SaveFile(CreateSeller.LogoFile, LogoName, null, "images", "seller-logos");

        
        await _applicationSigninManager.SignInAsync(_user, true);
        return Json(new JsonResultOperation(true, "شما با موفقیت به عنوان فروشنده انتخاب شدید"));

    }
}