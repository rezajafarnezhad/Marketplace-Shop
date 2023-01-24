using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface ICategoryVariantService : IGenericService<CategoryVarieant>
{

    Task<List<long>> GetCategoryVariants(long categoryId);
}
