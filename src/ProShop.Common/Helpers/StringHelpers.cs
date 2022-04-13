﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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

        public static string GenerateFileName(this IFormFile file)
        {
            return GenerateGuid() + Path.GetExtension(file.FileName);
        }
    }
}