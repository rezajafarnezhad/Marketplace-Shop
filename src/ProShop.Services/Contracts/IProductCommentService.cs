using ProShop.Entities;
using ProShop.ViewModels;
using ProShop.ViewModels.Product;
using ProShop.ViewModels.Product.ProductComments;

namespace ProShop.Services.Contracts;

public interface IProductCommentService : IGenericService<ProductComment>
{
    Task<List<ProductCommentForProductInfoViewModel>> GetCommentsByPagination(long productsId, int pageNumber, CommentSorting sortBy, SortingOrder orderBy);
}