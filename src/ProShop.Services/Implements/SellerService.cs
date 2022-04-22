using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;

namespace ProShop.Services.Implements;

public class SellerService : GenericService<Seller>, ISellerService
{
    private readonly DbSet<Seller> _sellers;

    public SellerService(IUnitOfWork uow)
        : base(uow)
    {
        _sellers = uow.Set<Seller>();
    }

    public override async Task<DuplicateColumns> AddAsync(Seller entity)
    {
        var result = new List<string>();

        if (await _sellers.AnyAsync(c => c.ShopName == entity.ShopName))
            result.Add(nameof(Seller.ShopName));

        if (await _sellers.AnyAsync(c => c.ShabaNumber == entity.ShabaNumber))
            result.Add(nameof(Seller.ShabaNumber));

        if (!result.Any())
            await base.AddAsync(entity);

        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };
    }

    public async Task<int> GetSellerCodeForCreateSeller()
    {
        var LastSeller = await _sellers.OrderByDescending(c => c.Id)
            .Select(c => c.SellerCode)
            .FirstOrDefaultAsync();

        return LastSeller + 1;
    }
}