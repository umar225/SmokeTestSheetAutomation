using Coursewise.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Coursewise.Api.Utility
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Keys
                               .SelectMany(key => context.ModelState[key]!.Errors.Select(x => x.ErrorMessage)).ToList();


                context.Result = new BadRequestObjectResult(new BaseModel
                {
                    success = false,
                    message = JsonConvert.SerializeObject(errors).Replace("\"", "")
                });
            }
        }
    }
}
