using Icarus.API.Infrastructure;
using Icarus.Model;
using Icarus.Model.User;
using Icarus.Service.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Icarus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthFilter))]
    public class UserController : ControllerBase
    {
        // Burada servisi çağırıyoruz
        private readonly IUserService userService;
        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

        // Tüm kullanıcıların listeleneceği metodun servis katmanından çağırıldığı kısım
        [HttpGet]
        public General<UserViewModel> GetUsers()
        {
            return userService.GetUsers();
        }

        // Kullanıcı ekleme metodunun servis katmanından çağırıldığı kısım
        [HttpPost]
        // Sadece giriş işlemi yapmış bir kullanıcı, kullanıcı ekleme işlemini gerçekleştirebiliyor
        [ServiceFilter(typeof(LoginFilter))]
        public General<UserViewModel> Insert([FromBody] UserViewModel newUser)
        {
            return userService.Insert(newUser);
        }

        // Kullanıcı güncelleme metodunun servis katmanından çağırıldığı kısım
        [HttpPut("{id}")]
        public General<UserViewModel> Update(int id, [FromBody] UserViewModel user)
        {
            return userService.Update(id, user);
        }

        // Kullanıcı silme metodunun servis katmanından çağırıldığı kısım
        [HttpDelete("{id}")]
        public General<UserViewModel> Delete(int id)
        {
            return userService.Delete(id);
        }
    }
}
