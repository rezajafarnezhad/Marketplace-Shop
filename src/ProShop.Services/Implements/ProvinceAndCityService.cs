using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;

namespace ProShop.Services.Implements;

public class ProvinceAndCityService : GenericService<ProvinceAndCity>, IProvinceAndCityService
{
    private readonly DbSet<ProvinceAndCity> _provinceAndCities;
    public ProvinceAndCityService(IUnitOfWork uow)
        : base(uow)
    {
        _provinceAndCities = uow.Set<ProvinceAndCity>();
    }


    public async Task<Dictionary<long, string>> GetProvincesToShowSelectBox()
    {
        return await _provinceAndCities.Where(c => c.ParentId == null)
            .ToDictionaryAsync(c => c.Id, c => c.Title);

    }

    public async Task<Dictionary<long, string>> GetCitiesByProvinceIdInSelectBox(long provinceId)
    {
        return await _provinceAndCities.Where(c => c.ParentId == provinceId)
            .ToDictionaryAsync(c => c.Id, c => c.Title);
    }

    public async Task<(long, long)> GetForSeedData()
    {
        var _Province = await _provinceAndCities.Where(c => c.ParentId == null)
            .Select(c=> new{c.Id,c.Title})
            .SingleOrDefaultAsync(c => c.Title == "اصفهان"); 
        
        
        var _City = await _provinceAndCities.Where(c => c.ParentId !=null)
            .Select(c=> new{c.Id,c.Title})
            .SingleOrDefaultAsync(c => c.Title == "کاشان");


        return (_Province.Id,_City.Id);
    }
}