using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;

namespace ProShop.Services.Implements;

public class ParcelPostService : GenericService<Entities.ParcalPost>, IParcelPostService
{
    private readonly DbSet<Entities.ParcalPost> _parcelPost;
    private readonly IMapper _mapper;

    public ParcelPostService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _parcelPost = uow.Set<Entities.ParcalPost>();
    }

   
}