using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;

namespace ProShop.Services.Implements;

public class ConsignmentService : GenericService<Consignment>, IConsignmentService
{

    private readonly DbSet<Consignment> _variants;
    private readonly IMapper _mapper;
    public ConsignmentService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _variants = uow.Set<Entities.Consignment>();
        _mapper = mapper;
    }
}
