﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using ProShop.ViewModels.Categories;

namespace ProShop.Services.Implements;

public class CategoryService : GenericService<Category>, ICategoryService
{

    private readonly DbSet<Category> _categories;
    public CategoryService(IUnitOfWork uow)
        : base(uow)
    {
        _categories = uow.Set<Category>();
    }


    public async Task<ShowCategoriesViewModel> GetCategories(ShowCategoriesViewModel model)
    {
        var categories = _categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(model.SearchCategories.Title))
            categories = categories.Where(c => c.Title.Contains(model.SearchCategories.Title.Trim()));

        if (!string.IsNullOrWhiteSpace(model.SearchCategories.Slug))
            categories = categories.Where(c => c.Slug.Contains(model.SearchCategories.Slug.Trim()));

        switch (model.SearchCategories.DeletedStatus)
        {
            case DeletedStatus.True:
                break;

            case DeletedStatus.OnlyDeleted:
                categories = categories.Where(c => c.IsDeleted);
                break;

            default:
                categories = categories.Where(c => !c.IsDeleted);
                break;

        }

        switch (model.SearchCategories.ShowInMenusStatus)
        {
            case ShowInMenusStatus.False:
                categories = categories.Where(c => !c.IsShowInMenus);
                break;

            case ShowInMenusStatus.True:
                categories = categories.Where(c => c.IsShowInMenus);
                break;

            default:
                break;
        }

        var paginationResult = await GenericPagination(categories,model.Pagination);

        return new()
        {
            Categories = await paginationResult.Query
                .Select(c => new ShowCategoryViewModel()
                {
                    Parent = c.ParentId != null ? c.ParentCategory.Title : "دسته اصلی",
                    ShowInMenus = c.IsShowInMenus,
                    Title = c.Title,
                    slug = c.Slug,
                    Picture = c.Picture ?? "بدون عکس"

                }).ToListAsync(),
            Pagination = paginationResult.Pagination

        };
    }

    public Dictionary<long, string> GetCategoriesToShowInSelectBox()
    {
        return _categories.ToDictionary(c => c.Id, c => c.Title);
    }

    public override async Task<DuplicateColumns> AddAsync(Category entity)
    {
        var result = new List<string>();

        if (await _categories.AnyAsync(c => c.Title == entity.Title))
            result.Add(nameof(Category.Title));

        if (await _categories.AnyAsync(c => c.Slug == entity.Slug))
            result.Add(nameof(Category.Slug));

        if (!result.Any())
            await base.AddAsync(entity);

        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };

    }
}