using System.Security.AccessControl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Bcpg.Sig;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Features;

namespace ProShop.web.Pages.AdminPanel.Feature
{
    public class IndexModel : PageBase
    {

        private readonly IFeatureService _featureService;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryFeatureService _categoryFeatureService;
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IFeatureService featureService, ICategoryService categoryService, IUnitOfWork unitOfWork, ICategoryFeatureService categoryFeatureService)
        {
            _featureService = featureService;
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
            _categoryFeatureService = categoryFeatureService;
        }


        public ShowFeaturesViewModel FeaturesViewModel { get; set; } = new();

        public void OnGet()
        {
            var categories = _categoryService.GetCategoriesToShowInSelectBox();
            FeaturesViewModel.SearchFeature.Categories = categories.CreateSelectListItem();
        }

        public async Task<IActionResult> OnGetGetDataTableAsync(ShowFeaturesViewModel featuresViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }

            return Partial("_List", await _featureService.GetCategoryFeatures(featuresViewModel));
        }

        public async Task<IActionResult> OnPostDelete(long FeatureId, long CategoryId)
        {
            var _categoryfeature = await _categoryFeatureService.GetCategoryFeature(FeatureId, CategoryId);
            if (_categoryfeature is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

            }

            _categoryFeatureService.Remove(_categoryfeature);
            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "ویژگی دسته بندی مورد نظر با موفقیت حذف شد"));

        }
        public async Task<IActionResult> OnGetAdd(long categoryId)
        {
            var categories = _categoryService.GetCategoriesToShowInSelectBox();
            var model = new AddFeatureViewModel()
            {
                CategoryId = categoryId,
                Categories = categories.CreateSelectListItem()
            };
            return Partial("Add", model);
        }


        public async Task<IActionResult> OnPostAdd(AddFeatureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }

            var _title = model.Title.Trim();
            var feature = await _featureService.FindByTitle(_title);
            if (feature is null)
            {
                //Add Feature
                await _featureService.AddAsync(new Entities.Feature()
                {
                    Title = _title,
                    CategoryFeatures = new List<CategoryFeature>()
                    {
                        new CategoryFeature()
                        {
                            CategoryId = model.CategoryId
                        }
                    }
                });
            }
            else
            {
                var categoryFeature = await _categoryFeatureService.GetCategoryFeature(feature.Id, model.CategoryId);
                if (categoryFeature is null)
                {
                    feature.CategoryFeatures.Add(new CategoryFeature()
                    {
                        CategoryId = model.CategoryId
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "ویژگی دسته بندی مورد نظر با موفقیت ذخیره شد"));
        }

        public async Task<IActionResult> OnGetAutocompleteSearch(string term)
        {
            return Json(await _featureService.AutocompleteSearch(term));
        }
    }
}


