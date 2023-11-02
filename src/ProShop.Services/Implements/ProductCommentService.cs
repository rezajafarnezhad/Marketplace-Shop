using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.Common.Helpers;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using ProShop.ViewModels.Product;
using ProShop.ViewModels.Product.ProductComments;

namespace ProShop.Services.Implements;

public class ProductCommentService : GenericService<Entities.ProductComment>, IProductCommentService
{
    private readonly DbSet<Entities.ProductComment> _producutComment;
    private readonly IMapper _mapper;

    public ProductCommentService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _producutComment = uow.Set<Entities.ProductComment>();
    }

    public async Task<List<ProductCommentForProductInfoViewModel>> GetCommentsByPagination(long productsId, int pageNumber, CommentSorting sortBy, SortingOrder orderBy)
    {
        var query = _producutComment
            .Where(c=>c.IsConfirmed)
            .Where(c => c.ProductId == productsId);

        #region OrderBy
        if (sortBy == CommentSorting.MostUseful)
        {
            query = query.OrderByDescending(c =>
                c.CommentScores.LongCount(v => v.IsLike) - c.CommentScores.LongCount(v => !v.IsLike));
        }
        else
        {
            query = query.CreateOrderByExpression(sortBy.ToString(),orderBy.ToString());
        }
        #endregion

        query = await GenericPaginationAsync(query, pageNumber, 1);
        return await _mapper.ProjectTo<ProductCommentForProductInfoViewModel>(query).ToListAsync();

    }
}