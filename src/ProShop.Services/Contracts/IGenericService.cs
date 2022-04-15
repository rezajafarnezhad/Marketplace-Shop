using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface IGenericService<TEntity> where TEntity : EntityBase, new()
{
    Task<DuplicateColumns> AddAsync(TEntity entity);
    Task<DuplicateColumns> Update(TEntity entity);
    void Remove(TEntity entity);
    void Remove(long id);
    Task<TEntity> FindByIdAsync(long id);
    Task<bool> IsExistsBy(string propertyToFilter, object propertyValue, long? id = null);
    Task SoftDelete(TEntity entity);
    Task Restore(TEntity entity);
}