using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProShop.Common.Attributes;

public class DivisibleBy10Attribute : BaseValidationAttribute, IClientModelValidator
{
    public DivisibleBy10Attribute()
    {
        ErrorMessage = "آخرین عدد قیمت باید 0 باشد";
    }

    protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
    {
        var displayName = validationContext.DisplayName;
        ErrorMessage = ErrorMessage.Replace("{0}", displayName);

        var price = value as int?;
        if (price is null || price % 10 == 0)
            return ValidationResult.Success;

        return new ValidationResult(ErrorMessage);
            
      
       
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        var displayName = context.ModelMetadata.ContainerMetadata
            .ModelType.GetProperty(context.ModelMetadata.PropertyName)
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault()?.Name;
        ErrorMessage = ErrorMessage.Replace("{0}", displayName);

        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-DivisibleBy10", ErrorMessage);
    }
}