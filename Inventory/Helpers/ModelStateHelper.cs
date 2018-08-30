using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Helpers
{
    public static class ModelStateHelper
    {
        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            return !modelState.IsValid ? modelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage ?? e.Exception.Message) : null;
        }

        public static string ErrorsToString(this ModelStateDictionary modelState)
        {
            return !modelState.IsValid ? string.Join(";", Errors(modelState)) : null;
        }
    }
}
