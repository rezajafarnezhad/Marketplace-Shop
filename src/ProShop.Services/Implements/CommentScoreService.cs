using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.DataLayer.Migrations;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Product;

namespace ProShop.Services.Implements;

public class CommentScoreService : GenericService<Entities.CommentScore>, ICommentScoreService
{
    private readonly DbSet<Entities.CommentScore> _commentScores;
    private readonly IMapper _mapper;
    public CommentScoreService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _commentScores = uow.Set<Entities.CommentScore>();
    }


    public async Task<List<LikedCommentByUserViewModel>> GetLikedCommentByUser(long userId, long[] commentIds)
    {
        return
            await _mapper.ProjectTo<LikedCommentByUserViewModel>(_commentScores
                .Where(c => c.UserId == userId)
                .Where(c => commentIds.Contains(c.ProductCommentId))).ToListAsync();
    }
}