namespace ProShop.Entities;

public class Garantee : EntityBase , IAuditableEntity
{
    public string Title { get; set; }
    public string FullTitle => $"گارانتی {MonthCount} ماهه {Title}";
    public byte MonthCount { get; set; }
    public string Picture { get; set; }
    public bool IsConfirmed { get; set; }
    public ICollection<ParcelPostItem> ParcelPostItems { get; set; }
  
} 
