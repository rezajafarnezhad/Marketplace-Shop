using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;

namespace ProShop.Services.Implements;

public class CommentsReportsService : GenericService<Entities.CommentsReports>, ICommentsReportsService
{
    private readonly DbSet<Entities.CommentsReports> _commentReports;
    private readonly IMapper _mapper;

    public CommentsReportsService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _commentReports = uow.Set<Entities.CommentsReports>();
    }

}