using Icarus.API.Infrastructure;
using Icarus.Model;
using Icarus.Model.User;
using Icarus.Service.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Icarus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IMemoryCache memoryCache;
        private readonly IUserService userService;

        public LoginController(IMemoryCache _memoryCache, IUserService _userService)
        {
            memoryCache = _memoryCache;
            userService = _userService;
        }

        [HttpPost]
        // Sisteme giriş işleminin gerçekleştirildiği yer
        public General<bool> Login([FromBody] LoginViewModel loginUser)
        {
            General<bool> response = new() { Entity = false };
            General<UserViewModel> result = userService.Login(loginUser);

            // login işleminin başarılı olup olmadığı kontrol ediliyor
            if (result.IsSuccess)
            {
                // Cache'te böyle bir kullanıcı yoksa cache'e kullanıcı ekleniyor
                if(!memoryCache.TryGetValue("LoginUser", out UserViewModel _loginUser))
                {
                    memoryCache.Set("LoginUser", result.Entity);
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
