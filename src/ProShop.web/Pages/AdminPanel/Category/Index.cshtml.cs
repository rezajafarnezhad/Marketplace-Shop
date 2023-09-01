using AutoMapper;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.DataLayer.Migrations;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Categories;
using ProShop.ViewModels.CategoryVaraints;

namespace ProShop.web.Pages.AdminPanel.Category
{
    public class IndexModel : PageBase
    {

        #region Ctor

        private readonly ICategoryService _categoryService;
        private readonly IUploadFileService _uploadFileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly IVariantService _variantService;
        private readonly ICategoryVariantService _categoryVariantService;
        private readonly IProductVariantService _productVariantService;

        public IndexModel(ICategoryService categoryService, IUnitOfWork unitOfWork, IUploadFileService uploadFileService, IBrandService brandService, IMapper mapper, IHtmlSanitizer htmlSanitizer, IVariantService variantService, ICategoryVariantService categoryVariantService, IProductVariantService productVariantService)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
            _uploadFileService = uploadFileService;
            _brandService = brandService;
            _mapper = mapper;
            _htmlSanitizer = htmlSanitizer;
            _variantService = variantService;
            _categoryVariantService = categoryVariantService;
            _productVariantService = productVariantService;
        }

        #endregion

        [BindProperty(SupportsGet = true)]
        public ShowCategoriesViewModel CategoriesViewModel { get; set; } = new();
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

            return Partial("_List", await _categoryService.GetCategories(CategoriesViewModel));
        }


        public async Task<IActionResult> OnGetAdd(long id = 0)
        {

            if (id > 0)
            {
                if (!await _categoryService.IsExistsBy(nameof(Entities.Category.Id), id))
                    return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }

            var model = new AddCategoryViewModel()
            {
                ParentId = id,
                MainCategories = _categoryService.GetCategoriesToShowInSelectBox()
                    .CreateSelectListItem(firstItemText: "خودش سر دسته اصلی است")

            };
            return Partial("Add", model);
        }

        public async Task<IActionResult> OnPostAdd(AddCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }

            string picturefileName = null;
            if (model.Picture.IsFileUploaded())
            {
                picturefileName = model.Picture.GenerateFileName();
            }

            var _category = _mapper.Map<Entities.Category>(model);

            _category.Description = _htmlSanitizer.Sanitize(_category.Description);

            if (model.ParentId is 0)
                _category.ParentId = null;

            _category.Picture = picturefileName;

            var result = await _categoryService.AddAsync(_category);
            if (!result.Ok)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.DuplicateErrorMessage)
                {
                    Data = result.Columns.SetDuplicateColumnsErrors<AddCategoryViewModel>()
                });
            }

            await _unitOfWork.SaveChangesAsync();
            await _uploadFileService.SaveFile(model.Picture, picturefileName, null, "images", "Categories");
            return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت ذخیره شد"));
        }

        public async Task<IActionResult> OnGetEdit(long Id)
        {
            var model = await _categoryService.GetForEdit(Id);
            if (model is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }
            model.MainCategories = _categoryService.GetCategoriesToShowInSelectBox(Id)
                .CreateSelectListItem(firstItemText: "خودش سر دسته اصلی است");
            return Partial("Edit", model);
        }

        public async Task<IActionResult> OnPostEdit(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }

            if (model.Id == model.ParentId)
            {
                return Json(new JsonResultOperation(false, "یک رکورد نمیتواند والد خودش باشد"));
            }


            var _category = await _categoryService.FindByIdWithIncludesAsync(model.Id, nameof(Entities.Category.categoryVarieants));
            if (_category is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }


            if (model.Picture.IsFileUploaded())
            {
                string picturefileName = null;
                var oldFileName = _category.Picture;
                picturefileName = model.Picture.GenerateFileName();
                _category.Picture = picturefileName;
                await _uploadFileService.SaveFile(model.Picture, picturefileName, oldFileName, "images", "Categories");
            }

            if (_category.categoryVarieants.Any() || _category.HasVariant)
            {
                model.IsVariantColor = _category.IsVariantColor;
            }


            _category.Title = model.Title;
            _category.Slug = model.Slug;
            _category.ParentId = model.ParentId is 0 ? null : model.ParentId;
            _category.Description = model.Description;
            _category.IsShowInMenus = model.IsShowInMenus;
            _category.CanAddFakeProduct = model.CanAddFakeProduct;
            _category.IsVariantColor = model.IsVariantColor;


            var result = await _categoryService.Update(_category);

            if (!result.Ok)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.DuplicateErrorMessage)
                {
                    Data = result.Columns.SetDuplicateColumnsErrors<EditCategoryViewModel>()
                });
            }
            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت ویرایش شد"));
        }


        public async Task<IActionResult> OnPostDelete(long elementId)
        {
            var _category = await _categoryService.FindByIdAsync(elementId);
            if (_category is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }
            await _categoryService.SoftDelete(_category);
            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت حذف شد"));
        }

        public async Task<IActionResult> OnGetAddBrand(long categoryId)
        {
            var model = new AddBrandCategoryViewModel()
            {
                CategoryId = categoryId,
                Brands = await _categoryService.GetCategoryBrands(categoryId)
            };
            return Partial("AddBrand", model);
        }
        public async Task<IActionResult> OnPostAddBrand(AddBrandCategoryViewModel model)
        {
            if (model.CategoryId < 1)
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));


            if (model.Brands.Count < 1)
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage));

            var _Category = await _categoryService.GetCategoryWithItsBrands(model.CategoryId);
            if (_Category is null)
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

            _Category.CategoryBrands.Clear();
            model.Brands = model.Brands.Distinct().ToList();
            var brandsInDictionary = new Dictionary<string, byte>();
            foreach (var item in model.Brands)
            {
                var spilitBrands = item.Split("|||");
                brandsInDictionary.Add(spilitBrands[0], byte.Parse(spilitBrands[1]));
            }
            var brands = await _brandService.GetBrandsByFullTitle(brandsInDictionary.Select(c => c.Key).ToList());
            if (brands.Count != model.Brands.Count)
                return Json(new JsonResultOperation(false));

            foreach (var item in brands)
            {
                _Category.CategoryBrands.Add(new CategoryBrand()
                {
                    BrandId = item.Key,
                    CommissionPercentage = brandsInDictionary[item.Value]

                });
            }

            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "برند های مورد نظر با موفقیت به دسته بندی مذکور اضافه گردید"));
        }

        public async Task<IActionResult> OnGetAutocompleteSearch(string term)
        {
            return Json(await _brandService.AutocompleteSearch(term));
        }



        public async Task<IActionResult> OnPostRestore(long elementId)
        {
            var _category = await _categoryService.FindByIdAsync(elementId);
            if (_category is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }
            await _categoryService.Restore(_category);
            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت بازگردانی شد"));
        }

        public async Task<IActionResult> OnPostDeletePicture(long elementId)
        {
            var _category = await _categoryService.FindByIdAsync(elementId);
            if (_category is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }

            var fileName = _category.Picture;
            _category.Picture = null;
            await _unitOfWork.SaveChangesAsync();
            _uploadFileService.DeleteFile(fileName, "images", "Categories");
            return Json(new JsonResultOperation(true, "تصویر دسته بندی مورد نظر با موفقیت حذف شد"));
        }




        public async Task<IActionResult> OnPostCheckForTitle(string title)
        {
            return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Title), title));
        }

        public async Task<IActionResult> OnPostCheckForSlug(string slug)
        {
            return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Slug), slug));
        }

        public async Task<IActionResult> OnPostCheckForTitleOnEdit(string title, long id)
        {
            return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Title), title, id));
        }

        public async Task<IActionResult> OnPostCheckForSlugOnEdit(string slug, long id)
        {
            return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Slug), slug, id));
        }


        public async Task<IActionResult> OnGetEditCategoryVariant(long categoryId)
        {

            if (!await _categoryService.IsExistsBy(nameof(Entities.Category.Id), categoryId))
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));


            var isVariantTypeColor = await _categoryService.IsVariantTypeColor(categoryId);

            if (isVariantTypeColor is null)
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));



            var variants = await _variantService.GetVariantsForEditCategoryVariants(isVariantTypeColor.Value);

            var selectedVariants = await _categoryVariantService.GetCategoryVariants(categoryId);

            var model = new EditCategoryVariantViewModel()
            {
                // CategoryId = categoryId,
                Variants = variants,
                SelectedVariants = selectedVariants,
                // برای مثال این دسته بندی 3 رنگ دارد
                // از کدام یک از این رنگ ها در بخش تنوع محصولات استفاده شده
                // آیدی اون تنوع ها رو برگشت میزنیم
                // که به ادمین اجازه ندیم که اون تنوع هارو از این دسته بندی حذف کنه
                AddedVariantsToProductVariant = await _productVariantService.GetAddedVariantsToProductVariants(selectedVariants, categoryId)
            };

            return Partial("_EditCategoryVariant", model);
        }

        public async Task<IActionResult> OnPostEditCategoryVariant(EditCategoryVariantViewModel model)
        {
            var category = await _categoryService.GetCategoryForEditVariant(model.CategoryId);

            if (category is null)
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

            if (category.IsVariantColor is null)
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

            var categoryVariantsIds = category.categoryVarieants.Select(c => c.VariantId).ToList();
            if (!await _variantService.CheckVariantsCountAddConfirmStatusForEditCategoryVariants(categoryVariantsIds, category.IsVariantColor.Value))
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

            }

            var addedVariantsForProductVariants = await _productVariantService.GetAddedVariantsToProductVariants(categoryVariantsIds, model.CategoryId);


            //Clear CategoryVariant

            foreach (var CategoryVariant in category.categoryVarieants)
            {
                if (addedVariantsForProductVariants.Contains(CategoryVariant.VariantId))
                    continue;


                category.categoryVarieants.Remove(CategoryVariant);
            }


            //Add CategoryVariant
            foreach (var variantId in model.SelectedVariants)
            {
                if (category.categoryVarieants.Any(c => c.VariantId == variantId))
                    continue;

                category.categoryVarieants.Add(new CategoryVarieant
                {
                    VariantId = variantId
                });
            }
            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "تنوع ها با موفقیت ثبت شدند"));


        }
    }
}
