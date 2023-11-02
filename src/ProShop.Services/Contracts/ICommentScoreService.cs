using ProShop.Entities;
using ProShop.ViewModels.Product;

namespace ProShop.Services.Contracts;

public interface ICommentScoreService : IGenericService<CommentScore>
{
    Task<List<LikedCommentByUserViewModel>> GetLikedCommentByUser(long userId, long[] commentIds);
}