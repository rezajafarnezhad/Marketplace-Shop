using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface IProvinceAndCityService : IGenericService<ProvinceAndCity>
{
    Task<Dictionary<long, string>> GetProvincesToShowSelectBox();
    Task<Dictionary<long, string>> GetCitiesByProvinceIdInSelectBox(long provinceId);
    Task<(long, long)> GetForSeedData();

}