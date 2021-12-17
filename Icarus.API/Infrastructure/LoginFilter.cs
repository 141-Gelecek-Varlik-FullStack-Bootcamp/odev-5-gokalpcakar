using Icarus.Model.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Icarus.API.Infrastructure
{
    public class LoginFilter : Attribute, IActionFilter
    {
        private readonly IMemoryCache memoryCache;
        public LoginFilter(IMemoryCache _memoryCache)
        {
            memoryCache = _memoryCache;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Kullanıcı giriş yapmamış ise 401 durum koduyla aşağıdaki mesajı dönüyoruz
            if (!memoryCache.TryGetValue("LoginUser", out UserViewModel _loginUser))
            {
                context.Result = new UnauthorizedObjectResult("Lütfen giriş yapınız");
            }
        }
    }
}
