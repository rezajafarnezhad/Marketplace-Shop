using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.Services.Implements;
using System.Formats.Asn1;

public class CategoryVariantService : GenericService<CategoryVarieant>, ICategoryVariantService
{
    private readonly DbSet<CategoryVarieant> _categoryVariant;
    private readonly IMapper _mapper;
    public CategoryVariantService(IUnitOfWork uow, IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _categoryVariant = uow.Set<CategoryVarieant>();
    }

    public async Task<List<long>> GetCategoryVariants(long categoryId)
    {
        return await _categoryVariant.Where(c=>c.CategoryId == categoryId).Select(c=>c.VariantId).ToListAsync();
    }
}
