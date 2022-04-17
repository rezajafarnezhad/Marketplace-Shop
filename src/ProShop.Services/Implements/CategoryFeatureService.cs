using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;

namespace ProShop.Services.Implements
{
    public class CategoryFeatureService : GenericService<CategoryFeature> , ICategoryFeatureService
    {
        private readonly DbSet<CategoryFeature> _categoryFeature;
        public CategoryFeatureService(IUnitOfWork uow)
            : base(uow)
        {
            _categoryFeature = uow.Set<CategoryFeature>();
        }


        public async Task<CategoryFeature> GetCategoryFeature(long featureId, long categoryId)
        {
            return await _categoryFeature.Where(c => c.CategoryId == categoryId)
                .SingleOrDefaultAsync(c => c.FeatureId == featureId);

        }
    }
}
