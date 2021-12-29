using Icarus.API.Infrastructure;
using Icarus.Model;
using Icarus.Model.User;
using Icarus.Service.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;

namespace Icarus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        //private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly IUserService userService;

        public LoginController(IDistributedCache _distributedCache, IUserService _userService)
        {
            distributedCache = _distributedCache;
            userService = _userService;
        }

        [HttpPost]
        // Sisteme giriş işleminin gerçekleştirildiği yer
        public General<bool> Login([FromBody] LoginViewModel loginUser)
        {
            var cachedData = distributedCache.GetString("LoginUser" );

            General<bool> response = new() { Entity = false };
            General<UserViewModel> result = userService.Login(loginUser);

            // login işleminin başarılı olup olmadığı kontrol ediliyor
            if (result.IsSuccess)
            {
                var cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5)
                };

                if (string.IsNullOrEmpty(cachedData))
                {
                    distributedCache.SetString("LoginUser", JsonConvert.SerializeObject(result.Entity), cacheOptions);
                }

                // işlemin başarılı olması durumundaki geri dönüş değerlerini belirliyoruz
                response.Entity = true;
                response.IsSuccess = true;
                response.SuccessfulMessage = "Giriş işlemi başarıyla gerçekleştirilmiştir";
            }
            else
            {
                // işlem başarısızsa aşağıdaki mesaj dönüyor
                response.ExceptionMessage = "Kullanıcı bilgilerinizi tekrar giriniz";
            }

            return response;
        }
    }
}
