using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProShop.Common.Helpers;

public static class EnumExtensions
{
    public static string GetEnumDisplayName(this Enum enumValue)
    {
        if (enumValue is null)
            return "";

        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.GetName();
    }
}