﻿using ProShop.Entities;
using ProShop.ViewModels.Categories;

namespace ProShop.Services.Contracts;

public interface ICategoryService : IGenericService<Category>
{
    Task<ShowCategoriesViewModel> GetCategories(ShowCategoriesViewModel model);
    Dictionary<long, string> GetCategoriesToShowInSelectBox(long? id=null);
    Task<EditCategoryViewModel> GetForEdit(long Id);
    Task<List<List<ShowCategoryForCreateProductViewModel>>> GetCategoriesForCreateProduct(long[] selectedCategoriesIds);
    Task<List<string>> GetCategoryBrands(long categoryId);
    Task<Category> GetCategoryWithItsBrands(long categotyId);
    Task<bool> CanAddFakeProduct(long categoryId);


    Task<(bool issuccessful, List<long> categoryIds)> GetCategoryParentIds(long categoryId);
    Task<Dictionary<long, string>> GetCategoriesWithNoChild();
    Task<Dictionary<long, string>> GetSellerCategories();
    Task<bool?> IsVariantTypeColor(long categoryId);
    Task<Category> GetCategoryForEditVariant(long catagoryId);

    /// <summary>
    /// باید محصولات صفحه مقایسه از یک نو دسته بندی باشند
    /// معیار اولین محصول هست که تایین میکند
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> CheckProductCategoryIdsInComparePage(params int[] productCodes);
}