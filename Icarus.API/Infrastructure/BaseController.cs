using Icarus.Model.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Icarus.API.Infrastructure
{
    public class BaseController : ControllerBase
    {
        //private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        public BaseController(IDistributedCache _distributedCache)
        {
            distributedCache = _distributedCache;
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
            var cachedData = distributedCache.GetString("LoginUser");
            var response = new UserViewModel();

            //if(memoryCache.TryGetValue("LoginUser", out UserViewModel _loginUser))
            //{
            //    response = _loginUser;
            //}

            if (!string.IsNullOrEmpty(cachedData))
            {
                response = JsonConvert.DeserializeObject<UserViewModel>(cachedData);
            }

            return response;
        }
    }
}
