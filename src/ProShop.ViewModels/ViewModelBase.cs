using System.ComponentModel.DataAnnotations;

namespace ProShop.ViewModels;



public enum DeletedStatus
{
    [Display(Name = "نمایش داده نشوند")]
    False,
    [Display(Name = "نمایش داده شوند")]

    True,
    [Display(Name = "فقط حذف شده ها")]

    OnlyDeleted
}

public class PaginationViewModel
{
    public int CurrentPage { get; set; } = 1;

    public byte Take { get; set; }

    public int PagesCount { get; set; }
    public int StartPage 
    {
        get
        {
            return CurrentPage - 3 < 1 ? 1 : CurrentPage - 3;
        }

    } 
    public int EndPage 
    {
        get
        {
            return CurrentPage + 3 > PagesCount ? PagesCount : CurrentPage + 3;
        }

    }
}
public class PaginationResultViewModel<T>
{
    public IQueryable<T> Query { get; set; }

    public PaginationViewModel Pagination { get; set; }
}