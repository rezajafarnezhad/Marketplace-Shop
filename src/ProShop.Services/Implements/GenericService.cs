using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using System.Globalization;

namespace ProShop.Services.Implements;

public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : EntityBase, new()
{
    private readonly IUnitOfWork _uow;
    private readonly DbSet<TEntity> _entities;

    protected GenericService(IUnitOfWork uow)
    {
        _uow = uow;
        _entities = uow.Set<TEntity>();
    }

    public virtual async Task<DuplicateColumns> AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
        return new DuplicateColumns();
    }


    public virtual async Task<DuplicateColumns> Update(TEntity entity)
    {
        _entities.Update(entity);
        return new DuplicateColumns();
    }


    public void Remove(TEntity entity)
        => _entities.Remove(entity);

    public void Remove(long id)
    {
        var tEntity = new TEntity();
        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty is null)
            throw new Exception("The entity doesn't have Id field!");
        idProperty.SetValue(tEntity, id, null);
        _uow.MarkAsDeleted(tEntity);
    }

    public async Task<TEntity> FindByIdAsync(long id)
        => await _entities.FindAsync(id);


    public async Task<bool> IsExistsBy(string propertyToFilter, object propertyValue, long? id = null)
    {
        var exp = ExpressionHelpers.CreateExpression<TEntity>(propertyToFilter, propertyValue);
        return await _entities
            .Where(x => id == null || x.Id != id)
            .AnyAsync(exp);
    }


    public async Task SoftDelete(TEntity entity)
        => entity.IsDeleted = true;

    public async Task Restore(TEntity entity)
        => entity.IsDeleted = false;

    public async Task<PaginationResultViewModel<T>> GenericPagination<T>(IQueryable<T> items, PaginationViewModel pagination)
    {
        if (pagination.CurrentPage < 1)
            pagination.CurrentPage = 1;

        var take = pagination.PageCount switch
        {
            PageCount.TwentyFive => 25,
            PageCount.Fifty => 50,
            PageCount.Hundred => 100,
            _ => 10,
        };


        var itemsCount = await items.LongCountAsync();
        var pagesCount = (int)Math.Ceiling(
            (decimal)itemsCount / take
        );
        if (pagesCount <= 0)
            pagesCount = 1;
        if (pagination.CurrentPage > pagesCount)
            pagination.CurrentPage = pagesCount;
        var skip = (pagination.CurrentPage - 1) * take;
        pagination.PagesCount = pagesCount;
        return new PaginationResultViewModel<T>
        {
            Pagination = pagination,
            Query = items.Skip(skip).Take(take)
        };
    }

    public async Task<bool> AnyAsync() => await _entities.AnyAsync();

    public async Task<TEntity> FindAsync(params object[] ids)
    {
        return await _entities.FindAsync(ids);
    }
    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _entities.AddRangeAsync(entities);
    }

    public Task<TEntity> FindByIdWithIncludesAsync(long Id , params string[] includes)
    {
        var query = _entities.AsQueryable();
        foreach(var include in includes)
        {
            query= query.Include(include);
        }
        return  query.SingleOrDefaultAsync(c=>c.Id == Id);
    }

    public void RemoveRange(List<TEntity> entities)
    {
        _entities.RemoveRange(entities);
    }
}