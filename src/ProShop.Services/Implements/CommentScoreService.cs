using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.DataLayer.Migrations;
using ProShop.Services.Contracts;

namespace ProShop.Services.Implements;

public class CommentScoreService : GenericService<Entities.CommentScore>, ICommentScoreService
{
    private readonly DbSet<CommentScore> _commentScores;
    private readonly IMapper _mapper;
    public CommentScoreService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _commentScores = uow.Set<CommentScore>();
    }


    
}