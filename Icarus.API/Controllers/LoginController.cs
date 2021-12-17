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
        public General<bool> Login([FromBody] LoginViewModel loginUser)
        {
            General<bool> response = new() { Entity = false };
            General<UserViewModel> result = userService.Login(loginUser);

            if (result.IsSuccess)
            {
                if(!memoryCache.TryGetValue("LoginUser", out UserViewModel _loginUser))
                {
                    memoryCache.Set("LoginUser", result.Entity);
                }

                response.Entity = true;
                response.IsSuccess = true;
                response.SuccessfulMessage = "Giriş işlemi başarıyla gerçekleştirilmiştir";
            }
            else
            {
                response.ExceptionMessage = "Kullanıcı bilgilerinizi tekrar giriniz";
            }

            return response;
        }
    }
}
