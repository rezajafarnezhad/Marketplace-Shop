using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.CategoryFeatures;

namespace ProShop.Services.Implements
{
    public class CategoryFeatureService : GenericService<CategoryFeature> , ICategoryFeatureService
    {
        private readonly DbSet<CategoryFeature> _categoryFeature;
        private readonly IMapper _mapper;
        public CategoryFeatureService(IUnitOfWork uow, IMapper mapper)
            : base(uow)
        {
            _mapper = mapper;
            _categoryFeature = uow.Set<CategoryFeature>();
        }


        public async Task<CategoryFeature> GetCategoryFeature(long featureId, long categoryId)
        {
            return await _categoryFeature.Where(c => c.CategoryId == categoryId)
                .SingleOrDefaultAsync(c => c.FeatureId == featureId);

        }

        public async Task<List<CategoryFeatureForCreateProductViewModel>> GetCategoryFeatures(long categoryId)
        {
            return await _mapper.ProjectTo<CategoryFeatureForCreateProductViewModel>
                (_categoryFeature.Where(c => c.CategoryId == categoryId)).ToListAsync();
        }

        public async Task<Dictionary<long, string>> GetCategoryFeatureBy(long categoryId)
        {
            return await _categoryFeature.Include(c=>c.Feature).Where(c => c.CategoryId == categoryId)
                .ToDictionaryAsync(c => c.FeatureId, c => c.Feature.Title);
        }

        public async Task<bool> CheckCategoryFeature(long categoryId, long featureId)
        {
            return await _categoryFeature.Where(c=>c.CategoryId == categoryId).AnyAsync(c => c.FeatureId == featureId);
        }


        public async Task<bool> CheckCategoryFeaturesCount(long categoryId, List<long> featuresIds)
        {
            var _featuresCount = await _categoryFeature.Where(c => c.CategoryId == categoryId)
                .CountAsync(c => featuresIds.Contains(c.FeatureId));
            return _featuresCount == featuresIds.Count();


        }
    }
}
