using Icarus.Model.User;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace Icarus.API.Infrastructure.DistCache
{
    public class DistCache : IDistCache
    {
        private readonly IDistributedCache distributedCache;
        public DistCache(IDistributedCache _distributedCache)
        {
            distributedCache = _distributedCache;
        }
        public void SetCache(LoginViewModel loginUser)
        {
            var cachedData = distributedCache.GetString("LoginUser");

            var cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5)
            };

            if (string.IsNullOrEmpty(cachedData))
            {
                distributedCache.SetString("LoginUser", JsonConvert.SerializeObject(loginUser), cacheOptions);
            }
        }

        // giriş yapmış kullanıcıyı dönüyoruz
        public UserViewModel GetCurrentUser()
        {
            var cachedData = distributedCache.GetString("LoginUser");
            var response = new UserViewModel();

            if (!string.IsNullOrEmpty(cachedData))
            {
                response = JsonConvert.DeserializeObject<UserViewModel>(cachedData);
            }

            return response;
        }
    }
}
