using System.ComponentModel.DataAnnotations;

namespace ProShop.Entities;

public enum CompanyType
{
    [Display(Name = "سهامی عام")]
    PublicStock,

    [Display(Name = "سهامی خاص")]
    PrivateEquity,

    [Display(Name = "مسئولیت محدود")]
    LimitedResponsibility,

    [Display(Name = "تعاونی")]
    Cooperative,

    [Display(Name = "تضامنی")]
    Solidarity
}