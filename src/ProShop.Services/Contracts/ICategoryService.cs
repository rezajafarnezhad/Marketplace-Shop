using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface ICategoryService : IGenericService<Category>
{
    Task<List<Category>> GetAll();
}