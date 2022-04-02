namespace ProShop.Entities;
public class Category : EntityBase, IAuditableEntity
{
    public string Title { get; set; }
    public long? ParentId { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string Picture { get; set; }
    public bool IsShowInMenus { get; set; } = false;


    public Category ParentCategory { get; set; }
    public ICollection<Category> ChildCategory { get; set; }

}
