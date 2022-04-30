namespace ProShop.ViewModels.Categories;

public class AddBrandCategoryViewModel
{
    public long CategoryId { get; set; }
    public List<string> Brands { get; set; } = new();
}