using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Ultils.Extensions
{
    public static class ModelStateDictionaryExtension
    {
        public static string GetError(this ModelStateDictionary modelState)
        {
            string result = string.Empty;

            modelState.Values.ToList().ForEach(value => {
                result += "\r\n";
                result += string.Join("\r\n", value.Errors.Select(e => e.ErrorMessage)); 
            });

            return result;
        }
    }
}
