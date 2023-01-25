using Microsoft.AspNetCore.Mvc;
using ProShop.ViewModels.Veriants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProShop.ViewModels.CategoryVaraints;

public class EditCategoryVariantViewModel
{
    [HiddenInput]
    public long CategoryId { get; set; }
    public List<ShowVariantInEditCategoryVariantViewModel> Variants { get; set; }
    public List<long> SelectedVariants { get; set; } = new();
    public List<long> AddedVariantsToProductVariant { get; set; } = new();


}


