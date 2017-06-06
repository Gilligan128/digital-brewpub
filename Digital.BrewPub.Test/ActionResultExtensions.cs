using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Digital.BrewPub.Test
{
    public static class ActionResultExtensions
    {
        public static TModel GetModel<TModel>(this IActionResult result)
        {
            return (TModel)((ViewResult)result).Model;
        }
    }
}
