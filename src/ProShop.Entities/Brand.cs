namespace ProShop.Entities;

public class Brand : EntityBase, IAuditableEntity
{
    public string TitleFa { get; set; }
    public string TitleEn { get; set; }
    public string FullTitle => $"{TitleFa} - {TitleEn}";
    public string Description { get; set; }
    public bool IsIranianBrand { get; set; }
    public string LogoPicture { get; set; }
    public string BrandRegistrationPicture { get; set; }
    public string JudiciaryLink { get; set; }
    public string BrandLinkEn { get; set; }
    public bool IsConfirmed { get; set; }

    /// <summary>
    /// فروشتده پیشنهاد دهنده این برند
    /// </summary>
    public long? SellerId { get; set; }
    public Seller Seller { get; set; }
    public ICollection<CategoryBrand> CategoryBrands { get; set; } = new List<CategoryBrand>();
    public ICollection<Product> Products{ get; set; }


}