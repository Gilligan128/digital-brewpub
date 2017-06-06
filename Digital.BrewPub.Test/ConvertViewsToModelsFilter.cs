using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Serialization;

namespace Digital.BrewPub.Test
{
    public class ConvertViewsToModelsFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
           if(context.Result is ViewResult)
            {
                ViewResult oldResult = (ViewResult)context.Result;
                JsonResult newResult = new JsonResult(oldResult.Model);
                newResult.StatusCode = oldResult.StatusCode;
                context.Result = newResult;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
           
        }
    }
}
