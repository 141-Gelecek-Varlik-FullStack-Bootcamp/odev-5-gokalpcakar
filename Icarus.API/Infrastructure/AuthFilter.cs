using Icarus.Model.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace Icarus.API.Infrastructure
{
    public class AuthFilter : Attribute, IActionFilter
    {
        private readonly IDistributedCache distributedCache;
        public AuthFilter(IDistributedCache _distributedCache)
        {
            distributedCache = _distributedCache;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var cachedData = distributedCache.GetString("LoginUser");
            var response = new UserViewModel();

            if (!string.IsNullOrEmpty(cachedData))
            {
                response = JsonConvert.DeserializeObject<UserViewModel>(cachedData);

                if (!response.IsAdmin)
                {
                    context.Result = new UnauthorizedObjectResult("Lütfen admin olarak giriş yapınız");
                }
            }
        }
    }
}
