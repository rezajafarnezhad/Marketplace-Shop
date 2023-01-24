using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProShop.ViewModels.Veriants;

public class ShowVariantInEditCategoryVariantViewModel
{
    public long Id { get; set; }
    public string Value { get; set; }
    public bool IsColor { get; set; }
    public string ColorCode { get; set; }
}
