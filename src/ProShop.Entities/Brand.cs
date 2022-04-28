namespace ProShop.Entities;

public class Brand : EntityBase, IAuditableEntity
{
    public string TitleFa { get; set; }
    public string TitleEn { get; set; }
    public string Description { get; set; }
    public bool IsIranianBrand { get; set; }
    public string LogoPicture { get; set; }
    public string BrandRegistrationPicture { get; set; }
    public string JudiciaryLink { get; set; }
    public string BrandLinkEn { get; set; }

}