using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface IGenericService<TEntity> where TEntity : EntityBase, new()
{
    Task<DuplicateColumns> AddAsync(TEntity entity);
    Task<DuplicateColumns> Update(TEntity entity);
    void Remove(TEntity entity);
    void Remove(long id);
    Task<TEntity> FindByIdAsync(long id);
    Task<bool> IsExistsByIdAsync(long id);
    Task SoftDelete(TEntity entity);
}