﻿using AutoMapper;
using DNTPersianUtils.Core;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.ViewModels.FeatureConstantValue;

namespace ProShop.Services.Implements;

public class FeatureConstantValuesService : GenericService<FeatureConstantValue>, IFeatureConstantValuesService
{
    private readonly DbSet<FeatureConstantValue> _featureConstant;
    private readonly IMapper _mapper;
    public FeatureConstantValuesService(IUnitOfWork uow, IMapper mapper):base(uow)
    {
        _mapper = mapper;
        _featureConstant = uow.Set<FeatureConstantValue>();
    }


    public async Task<ShowFeatureConstantValuesViewModel> GetFeatureConstants(ShowFeatureConstantValuesViewModel model)
    {
        var _featureConstants = _featureConstant.AsNoTracking().AsQueryable();


        #region Search

        var searchFeatureId = model.SearchFeatureConstant.FeatureId;
        if (searchFeatureId != 0)
        {
            _featureConstants = _featureConstants.Where(c => c.FeatureId == searchFeatureId);
        }

        _featureConstants = _featureConstants.CreateSearchExpressions(model.SearchFeatureConstant,false);
        #endregion

        #region Sorting
        _featureConstants = _featureConstants.CreateOrderByExpression(model.SearchFeatureConstant.Sorting.ToString(),model.SearchFeatureConstant.SortingOrder.ToString());

        #endregion

        var paginationResult = await GenericPagination(_featureConstants, model.Pagination);

        return new ShowFeatureConstantValuesViewModel()
        {
            ShowFeatureConstantValues = await _mapper.ProjectTo<ShowFeatureConstantValueViewModel>(paginationResult.Query).ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }
}