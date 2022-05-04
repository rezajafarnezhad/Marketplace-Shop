using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Brands;
using ProShop.ViewModels.Product;

namespace ProShop.web.Pages.SellerPanel.Product;

public class CreateModel : SellerPanelBase
{

    private readonly ICategoryService _categoryService;
    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadFileService _fileService;
    private readonly ISellerService  _sellerService;
    public AddProductViewModel Product { get; set; }
    public CreateModel(ICategoryService categoryService, IBrandService brandService, IMapper mapper, IUnitOfWork unitOfWork, IUploadFileService fileService, ISellerService sellerService)
    {
        _categoryService = categoryService;
        _brandService = brandService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _sellerService = sellerService;
    }

    public async Task<IActionResult> OnGetGetCategories(long[]? selectedCategoriesIds)
    {
        var result = await _categoryService.GetCategoriesForCreateProduct(selectedCategoriesIds);
        return Partial("_SelectProductCategoryPartial", result);
    }

    public async Task<IActionResult> OnGetGetCategoryBrands(long categoryId)
    {
        if (categoryId < 1)
            return Json(new JsonResultOperation(false));

        var Brands = await _brandService.GetBrandsByCategoryId(categoryId);
        return Json(new JsonResultOperation(true, "")
        {
            Data = Brands

        });

    }

    public async Task<IActionResult> OnGetCanAddFakeProduct(long categoryId)
    {

        return Json(new JsonResultOperation(true, "")
        {
            Data = await _categoryService.CanAddFakeProduct(categoryId)
        });
    }


    public async Task<IActionResult> OnGetRequestForAddBrand(long categoryId)
    {
        var model = new AddBrandBySelllerViewModel()
        {
            CategoryId = categoryId
        };
        return Partial("_RequestForAddBrandPartial",model);
    }
    public async Task<IActionResult> OnPostCheckForTitleFa(string titlefa)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleFa), titlefa));

    }
    public async Task<IActionResult> OnPostCheckForTitleEn(string titleEn)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleEn), titleEn));
    }
    public async Task<IActionResult> OnPostRequestForAddBrand(AddBrandBySelllerViewModel model)
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
        _Brand.IsConfirmed = false;
        _Brand.SellerId = await _sellerService.GetSellerId(User.Identity.GetUserId().Value);
        _Brand.CategoryBrands = new List<CategoryBrand>()
        {
            new CategoryBrand()
            {
                CategoryId = model.CategoryId,
            }
        };

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


        return Json(new JsonResultOperation(true, "برند ثبت شد و پس از تایید کارشناسان قابل دسترسی است، مراتب از طریق ایمیل به شما اطلاع رسانی خواهد شد"));
    }

    public IActionResult OnPostUploadSpecialtyCheckImages([IsImage("")]IFormFile file)
    {
        if (ModelState.IsValid && file.IsFileUploaded())
        {
            var imageFileName = file.GenerateFileName();
            _fileService.SaveFile(file, imageFileName, null, "images", "Products", "SpecialtyCheckImage");
            return Json(new
            {
                location = $"/images/Products/SpecialtyCheckImage/{imageFileName}"
            });
        }
        return Json(false);
    } 
    
    public IActionResult OnPostUploadShortDescriptionImages([IsImage("")]IFormFile file)
    {
        if (ModelState.IsValid && file.IsFileUploaded())
        {
            var imageFileName = file.GenerateFileName();
            _fileService.SaveFile(file, imageFileName, null, "images", "Products", "short-description-images");
            return Json(new
            {
                location = $"/images/Products/short-description-images/{imageFileName}"
            });
        }
        return Json(false);
    }
}