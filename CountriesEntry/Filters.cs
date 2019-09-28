using CountriesEntry.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountriesEntry
{
    public class ValidateCountry : ActionFilterAttribute
    {
        private readonly RequestInfo info;

        public ValidateCountry(RequestInfo info)
        {
            this.info = info;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(info.Username == "2")
            {
                context.Result = new BadRequestObjectResult("Not Allowed for user-2");
            }
            var parameter = context.ActionArguments.FirstOrDefault(e => e.Value is Country);
            if (parameter.Key != null)
            {
                var country = parameter.Value as Country;

                if (country == null)
                {
                    context.Result = new BadRequestObjectResult("Country is null");
                }

                if (country.CountryCode == country.Name)
                {
                    context.Result = new BadRequestObjectResult("Country code equals country name!");
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }
    }

    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public CustomExceptionFilter(
            IModelMetadataProvider modelMetadataProvider)
        {
            _modelMetadataProvider = modelMetadataProvider;
        }

        public void OnException(ExceptionContext context)
        {
            var result = new ViewResult { ViewName = "Error" };
            result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
            result.ViewData.Add("Exception", context.Exception);

            // Here we can pass additional detailed data via ViewData
            context.ExceptionHandled = true; // mark exception as handled
            context.Result = result;
        }
    }

    public class AddHeaderFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add(
                "OnResultExecuting",
                "This header was added by result filter.");
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }

    public class CacheResourceFilter : IResourceFilter
    {
        private static readonly Dictionary<string, object> _cache
                = new Dictionary<string, object>();
        private string _cacheKey;

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString();
            if (_cache.ContainsKey(_cacheKey))
            {
                var cachedValue = _cache[_cacheKey] as string;
                if (cachedValue != null)
                {
                    context.Result = new ContentResult()
                    { Content = cachedValue };
                }
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (!string.IsNullOrEmpty(_cacheKey) && !_cache.ContainsKey(_cacheKey))
            {
                var result = context.Result as ContentResult;
                if (result != null)
                {
                    _cache.Add(_cacheKey, result.Content);
                }
            }
        }
    }
}
