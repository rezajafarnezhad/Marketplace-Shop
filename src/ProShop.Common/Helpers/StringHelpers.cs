using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProShop.Common.Constants;

namespace ProShop.Common.Helpers
{
    public static class StringHelpers
    {

        public static bool IsEmail(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return input.Contains('@');
        }


        public static string GenerateGuid() => Guid.NewGuid().ToString("N");

        public static List<string> SetDuplicateColumnsErrors<T>(this List<string> duplicateColumns)
        {
            var result = new List<string>();
            foreach (var itemColumn in duplicateColumns)
            {
                var columndName = typeof(T).GetProperty(itemColumn)!
                    .GetCustomAttribute<DisplayAttribute>()!.Name;
                result.Add($"این {columndName} قبلا در سیستم ثبت شده است");

            }

            return result;
        }
        public static void CheckStringInputs<T>(this ModelStateDictionary modelState, List<string> properties, T model)
        {
            foreach (var property in properties)
            {
                var currentProperty = typeof(T).GetProperty(property);
                var propertyValue = currentProperty.GetValue(model);
                if (string.IsNullOrWhiteSpace(propertyValue?.ToString()))
                {
                    var propertyDisplayName = currentProperty!
                        .GetCustomAttribute<DisplayAttribute>()!.Name;
                    modelState.AddModelError(property,
                        AttributesErrorMessages.RequiredMessage.Replace("{0}", propertyDisplayName));
                }
            }
        }

        public static string GenerateFileName(this IFormFile file)
        {
            return GenerateGuid() + Path.GetExtension(file.FileName);
        }

        public static string ConvertToEasternArabicNumerals(string input)
        {
            System.Text.UTF8Encoding utf8Encoder = new UTF8Encoding();
            System.Text.Decoder utf8Decoder = utf8Encoder.GetDecoder();
            System.Text.StringBuilder convertedChars = new System.Text.StringBuilder();
            char[] convertedChar = new char[1];
            byte[] bytes = new byte[] { 217, 160 };
            char[] inputCharArray = input.ToCharArray();
            foreach (char c in inputCharArray)
            {
                if (char.IsDigit(c))
                {
                    bytes[1] = Convert.ToByte(160 + char.GetNumericValue(c));
                    utf8Decoder.GetChars(bytes, 0, 2, convertedChar, 0);
                    convertedChars.Append(convertedChar[0]);
                }
                else
                {
                    convertedChars.Append(c);
                }
            }
            return convertedChars.ToString();
        }
        public static string LatinNumToArabic(string str)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("0", "٠");
            dic.Add("1", "١");
            dic.Add("2", "٢");
            dic.Add("3", "٣");
            dic.Add("4", "٤");
            dic.Add("5", "٥");
            dic.Add("6", "٦");
            dic.Add("7", "٧");
            dic.Add("8", "٨");
            dic.Add("9", "٩");
            foreach (KeyValuePair<string, string> entry in dic)
            {
                str = str.Replace(entry.Key, entry.Value);
            }
            return str;
        }

        public static string ToShowGuaranteeFullTitle(this string input)
        {
            if (input.Contains("0 ماهه"))
            {
                return "گارانتی اصالت و سلامت فیزیکی";
            }

            return input;
        }
    }
}
