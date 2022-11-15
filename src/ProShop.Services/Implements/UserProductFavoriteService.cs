using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;

namespace ProShop.Services.Implements;

public class UserProductFavoriteService : GenericService<UserProductFavorite>, IUserProductFavoriteService
{
    private readonly DbSet<UserProductFavorite> _userProductFavorites;
    private readonly IMapper _mapper;
    public UserProductFavoriteService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _userProductFavorites = uow.Set<UserProductFavorite>();
        _mapper = mapper;
    }


}