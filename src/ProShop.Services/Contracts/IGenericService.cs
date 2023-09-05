using ProShop.Common.Helpers;
using ProShop.Entities;
using ProShop.ViewModels.Cart;

namespace ProShop.Services.Contracts;

public interface IGenericService<TEntity> where TEntity : EntityBase, new()
{
    Task<DuplicateColumns> AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task<DuplicateColumns> Update(TEntity entity);
    void Remove(TEntity entity);
    void Remove(long id);
    Task<TEntity> FindByIdAsync(long id);
    Task<TEntity> FindAsync(params object[] ids);
    Task<bool> IsExistsBy(string propertyToFilter, object propertyValue, long? id = null);
    Task SoftDelete(TEntity entity);
    Task Restore(TEntity entity);
    Task<bool> AnyAsync();
    void RemoveRange(List<TEntity> entities);
    Task<TEntity> FindByIdWithIncludesAsync(long Id, params string[] includes);


    
}