using AutoMapper;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.Services.Implements;
using ProShop.ViewModels.Brands;
using ProShop.ViewModels.CategoryFeatures;
using ProShop.ViewModels.FeatureConstantValue;
using ProShop.ViewModels.Product;
using System.Text;

namespace ProShop.web.Pages.SellerPanel.Product;

public class CreateModel : SellerPanelBase
{

    private readonly ICategoryService _categoryService;
    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadFileService _fileService;
    private readonly ISellerService _sellerService;
    private readonly ICategoryFeatureService _categoryFeatureService;
    private readonly IFeatureConstantValuesService _featureConstantValuesService;
    private readonly IViewRenderService _viewRenderService;
    private readonly IHtmlSanitizer _htmlSanitizer;
    private readonly IProductService _productService;
    private readonly ICategoryBrandService _categoryBrandService;
    private readonly IProductShortLinkService _productShortLinkService;


    public CreateModel(ICategoryService categoryService,
        IBrandService brandService, IMapper mapper,
        IUnitOfWork unitOfWork, IUploadFileService fileService,
        ISellerService sellerService, ICategoryFeatureService categoryFeatureService,
        IFeatureConstantValuesService featureConstantValuesService,
        IViewRenderService viewRenderService, IHtmlSanitizer htmlSanitizer, IProductService productService, ICategoryBrandService categoryBrandService, IProductShortLinkService productShortLinkService)
    {
        _categoryService = categoryService;
        _brandService = brandService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _sellerService = sellerService;
        _categoryFeatureService = categoryFeatureService;
        _featureConstantValuesService = featureConstantValuesService;
        _viewRenderService = viewRenderService;
        _htmlSanitizer = htmlSanitizer;
        _productService = productService;
        _categoryBrandService = categoryBrandService;
        _productShortLinkService = productShortLinkService;
    }



    public AddProductViewModel Product{ get; set; }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPost(AddProductViewModel Product)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, "")
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        
        var ProductToAdd = _mapper.Map<Entities.Product>(Product);

        var categoriesToAdd = await _categoryService.GetCategoryParentIds(Product.MainCategoryId);
        if (!categoriesToAdd.issuccessful)
            return Json(new JsonResultOperation(false));


        if (!await _categoryBrandService.CheckCategoryBrand(Product.MainCategoryId, Product.BrandId))
            return Json(new JsonResultOperation(false));

        ProductToAdd.SelerId = await _sellerService.GetSellerId(User.Identity.GetLoggedUserId());
        ProductToAdd.Slug = Product.PersianTitle.Replace(" ","-");
        var ShortLink = await _productShortLinkService.GetProductShortLinkForCreateProduct();
        ProductToAdd.ProductShortLinkId = ShortLink.Id;
        ShortLink.IsUsed = true;

        ProductToAdd.ShortDescription = _htmlSanitizer.Sanitize(ProductToAdd.ShortDescription);
        ProductToAdd.SpecialCheck = _htmlSanitizer.Sanitize(ProductToAdd.SpecialCheck);
        ProductToAdd.ProductCode = await _productService.GetProductCode();

        // main Category Id
        ProductToAdd.MainCategoryId =  categoriesToAdd.categoryIds.First();


        foreach (var item in categoriesToAdd.categoryIds)
        {
            ProductToAdd.productCategories.Add(new ProductCategory
            {
                CategoryId = item,
            });
        }

        if (!await _categoryService.CanAddFakeProduct(Product.MainCategoryId))
            ProductToAdd.IsFake = false;

        foreach (var Picture in Product.ProductImageFiles)
        {
            if (Picture.IsFileUploaded())
            {
                var fileName = Picture.GenerateFileName();
                ProductToAdd.ProductMedia.Add(new ProductMedia()
                {
                    FileName = fileName,
                    IsVideo = false,
                });
            }

        }

        if (Product.ProductVideoFiles is not null)
        {
            foreach (var Video in Product.ProductVideoFiles)
            {
                if (Video.IsFileUploaded())
                {
                    var fileName = Video.GenerateFileName();
                    ProductToAdd.ProductMedia.Add(new ProductMedia()
                    {
                        FileName = fileName,
                        IsVideo = true
                    });
                }
            }
        }

        #region Non ConstantValue

        var featureIds = new List<long>();
        var ProductFeatureValueInputs = Request.Form.Where(c => c.Key.StartsWith("ProductFeatureValue")).ToList();
        foreach (var item in ProductFeatureValueInputs)
        {
            if (long.TryParse(item.Key.Replace("ProductFeatureValue", String.Empty), out var featureId))
                featureIds.Add(featureId);

            else
                return Json(new JsonResultOperation(false, ""));
        }


        if (await _featureConstantValuesService.CheckNonConstantValue(Product.MainCategoryId, featureIds))
            return Json(new JsonResultOperation(false, ""));

        foreach (var item in ProductFeatureValueInputs)
        {


            if (long.TryParse(item.Key.Replace("ProductFeatureValue", String.Empty), out var featureId))
            {
                var trimmedValue = item.Value.ToString().Trim();
                if (ProductToAdd.ProductFeatures.All(c => c.FeatureId != featureId))
                {
                    if (trimmedValue.Length > 0)
                    {
                        ProductToAdd.ProductFeatures.Add(new ProductFeature()
                        {
                            FeatureId = featureId,
                            Value = trimmedValue,
                        });
                    }

                }

            }
        }

        #endregion

        #region ConstantValue

        var featureConstantValueIds = new List<long>();
        var ProductFeatureConstantValueInputs = Request.Form.Where(c => c.Key.StartsWith("ProductFeatureConstantValue")).ToList();

        foreach (var item in ProductFeatureConstantValueInputs)
        {
            if (long.TryParse(item.Key.Replace("ProductFeatureConstantValue", String.Empty), out var featureId))
                featureConstantValueIds.Add(featureId);

            else
                return Json(new JsonResultOperation(false, ""));
        }

        featureIds = featureIds.Concat(featureConstantValueIds).ToList();

        if (!await _categoryFeatureService.CheckCategoryFeaturesCount(Product.MainCategoryId, featureIds))
            return Json(new JsonResultOperation(false, ""));


        if (!await _featureConstantValuesService.CheckConstantValue(Product.MainCategoryId, featureConstantValueIds))
            return Json(new JsonResultOperation(false, ""));


        var featureConstantValues = await _featureConstantValuesService.GetFeatureConstantValuesForCreateProduct(Product.MainCategoryId);
        foreach (var item in ProductFeatureConstantValueInputs)
        {
            if (long.TryParse(item.Key.Replace("ProductFeatureConstantValue", String.Empty), out var featureId))
            {
                var s = item.Value;
                if (item.Value.Count > 0)
                {
                    var valueToAdd = new StringBuilder();
                    foreach (var value in item.Value)
                    {
                        var trimmedValue = value.Trim();
                        if (featureConstantValues.Where(c=>c.FeatureId == featureId).Any(c=>c.Value == trimmedValue))
                        {
                            valueToAdd.Append(trimmedValue + "|||");
                        }
                    }
                    if (ProductToAdd.ProductFeatures.All(c => c.FeatureId != featureId))
                    {
                        if (valueToAdd.ToString().Length > 0)
                        {
                            ProductToAdd.ProductFeatures.Add(new ProductFeature()
                            {
                                FeatureId = featureId,
                                Value = valueToAdd.ToString().Substring(0, valueToAdd.Length - 3)
                            });
                          
                        }
                    }

                }

            }
        }

        #endregion

        await _productService.AddAsync(ProductToAdd);
        await _unitOfWork.SaveChangesAsync();


        var ProductPictures = ProductToAdd.ProductMedia.Where(c => !c.IsVideo).ToList();
        for (int i = 0; i < ProductPictures.Count; i++)
        {
            var currentPicture = Product.ProductImageFiles[i];
            if (currentPicture.IsFileUploaded())
                await _fileService.SaveFile(currentPicture, ProductPictures[i].FileName, null, "images", "Products");


        }

        if (Product.ProductVideoFiles is not null)
        {
            var ProductVideos = ProductToAdd.ProductMedia.Where(c => c.IsVideo).ToList();
            for (int i = 0; i < ProductVideos.Count; i++)
            {
                var currentVideo = Product.ProductVideoFiles[i];
                if (currentVideo.IsFileUploaded())
                    await _fileService.SaveFile(currentVideo, ProductVideos[i].FileName, null, "Videos", "Products");

            }

        }


        return Json(new JsonResultOperation(true, "محصول مورد نظر با موفقیت ایجاد شد"));
        
         

    }
    public async Task<IActionResult> OnGetGetCategories(long[]? selectedCategoriesIds)
    {
        var result = await _categoryService.GetCategoriesForCreateProduct(selectedCategoriesIds);
        return Partial("_SelectProductCategoryPartial", result);
    }



    public async Task<IActionResult> OnGetCategoryInfo(long categoryId)
    {
        if (categoryId < 1)
            return Json(new JsonResultOperation(false));

        var CategoryFeatureModel = new ProductFeaturesForCreateProductViewModel()
        {
            Features = await _categoryFeatureService.GetCategoryFeatures(categoryId),
            FeaturesConstantValues = await _featureConstantValuesService.GetFeatureConstantValuesByCategoryId(categoryId)
        };

        var model = new
        {
            Brands = await _brandService.GetBrandsByCategoryId(categoryId),
            canAddFakeProduct = await _categoryService.CanAddFakeProduct(categoryId),
            CategoryFeaturs =
                await _viewRenderService.RenderViewToStringAsync("~/Pages/SellerPanel/Product/_showCategoryFeaturesPartial.cshtml", CategoryFeatureModel)
        };

        return Json(new JsonResultOperation(true, string.Empty)
        {
            Data = model
        });

    }

    public async Task<IActionResult> OnGetRequestForAddBrand(long categoryId)
    {
        var model = new AddBrandBySelllerViewModel()
        {
            CategoryId = categoryId
        };
        return Partial("_RequestForAddBrandPartial", model);
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

    public IActionResult OnPostUploadSpecialtyCheckImages([IsImage] IFormFile file)
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
  
    public async Task<IActionResult> OnGetGetCommissionPercentage(long brandId , long categoryid)
    {
        if(brandId <1 || categoryid < 1)
            return Json(new JsonResultOperation(false));

        var data = await _categoryBrandService.GetCommissionPercentage(brandId, categoryid);

        if(data.Item1 is false)
            return Json(new JsonResultOperation(false));


        return Json(new JsonResultOperation(true,String.Empty) { 
        Data =data.Item2
        });
    }

    public IActionResult OnPostUploadShortDescriptionImages([IsImage] IFormFile file)
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