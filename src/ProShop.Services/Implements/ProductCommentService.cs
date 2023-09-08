using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;

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

}