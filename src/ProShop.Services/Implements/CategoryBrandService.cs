using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;

namespace ProShop.Services.Implements;

public class CategoryBrandService : GenericService<Entities.CategoryBrand>, ICategoryBrandService
{
    private readonly DbSet<Entities.CategoryBrand> _categoryBrands;
    private readonly IMapper _mapper;
    public CategoryBrandService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _categoryBrands = uow.Set<Entities.CategoryBrand>();
    }
    public async Task<bool> CheckCategoryBrand(long categoryId, long brandId)
    {
        return await _categoryBrands.Where(c => c.CategoryId == categoryId)
            .AnyAsync(c => c.BrandId == brandId);
    }

    public async Task<(bool,byte)> GetCommissionPercentage(long brandId, long categoryId)
    {
        var data = await _categoryBrands.Where(c => c.CategoryId == categoryId && c.BrandId == brandId).SingleOrDefaultAsync();

        return (data !=null,data?.CommissionPercentage ?? 0);

    }
}