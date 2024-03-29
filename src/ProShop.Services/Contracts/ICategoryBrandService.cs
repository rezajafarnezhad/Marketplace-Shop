﻿using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface ICategoryBrandService : IGenericService<CategoryBrand>
{

    Task<bool> CheckCategoryBrand(long categoryId, long brandId);
    Task<(bool, byte)> GetCommissionPercentage(long brandId, long categoryId);
}