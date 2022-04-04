﻿using System.ComponentModel.DataAnnotations;

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