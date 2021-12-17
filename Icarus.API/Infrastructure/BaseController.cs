using Icarus.Model.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Icarus.API.Infrastructure
{
    public class BaseController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        public BaseController(IMemoryCache _memoryCache)
        {
            memoryCache = _memoryCache;
        }
        // GetCurrentUser metodunu erişilebilir yapıyoruz
        public UserViewModel CurrentUser
        {
            get
            {
                return GetCurrentUser();
            }
        }

        // giriş yapmış kullanıcıyı dönüyoruz
        private UserViewModel GetCurrentUser()
        {
            var response = new UserViewModel();

            if(memoryCache.TryGetValue("LoginUser", out UserViewModel _loginUser))
            {
                response = _loginUser;
            }

            return response;
        }
    }
}
