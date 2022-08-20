using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProShop.Common.Attributes;


public class IsImageAttribute : BaseValidationAttribute, IClientModelValidator
{
    private readonly bool _multiplePicture;

    private readonly string[] _allowExtensions = new[]
    {
        "image/png",
        "image/jpeg",
        "image/bmp",
        "image/gif"
    };
   

    public IsImageAttribute(bool multiplePicture =false)
    {
        _multiplePicture = multiplePicture;
        ErrorMessage = "{0} حتما باید عکس باشد";
    }


    protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
    {
        var displayName = validationContext.DisplayName;
        ErrorMessage = ErrorMessage.Replace("{0}", displayName);

        if (_multiplePicture)
        {
            ErrorMessage = ErrorMessage.Replace("باشد", "باشند");
        }

        var files = value as List<IFormFile>;
        if (files != null && files.Count > 0)
        {
            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
                if (!_allowExtensions.Contains(file.ContentType))
                {
                    return new ValidationResult(ErrorMessage);
                }
                try
                {
                    var img = Image.FromStream(file.OpenReadStream());
                    if (img.Width <= 0)
                        return new ValidationResult(ErrorMessage);
                }
                catch
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
        }
        return ValidationResult.Success;
    }
    public void AddValidation(ClientModelValidationContext context)
    {
        var displayName = context.ModelMetadata.ContainerMetadata
            .ModelType.GetProperty(context.ModelMetadata.PropertyName)
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault()?.Name;
        ErrorMessage = ErrorMessage.Replace("{0}", displayName);

        if (_multiplePicture)
        {
            ErrorMessage = ErrorMessage.Replace("باشد", "باشند");
        }

        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-isImage", ErrorMessage);
        MergeAttribute(context.Attributes, "data-val-whitelistextensions",
            string.Join(",", _allowExtensions));
    }
}