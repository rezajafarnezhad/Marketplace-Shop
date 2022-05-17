﻿namespace ProShop.Entities;

public class Feature : EntityBase , IAuditableEntity
{
    public string Title { get; set; }

    public ICollection<CategoryFeature> CategoryFeatures { get; set; } = new List<CategoryFeature>();
    public ICollection<FeatureConstantValue> FeatureConstantValues { get; set; } = new List<FeatureConstantValue>();

}