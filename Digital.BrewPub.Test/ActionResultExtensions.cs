using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Digital.BrewPub.Test
{
    public static class ActionResultExtensions
    {
        public static TModel GetModel<TModel>(this IActionResult result)
        {
            return (TModel)((ViewResult)result).Model;
        }

        public static async Task<TModel> GetModelAsync<TModel>(this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TModel>(content);
            return model;
        }
    }
}
