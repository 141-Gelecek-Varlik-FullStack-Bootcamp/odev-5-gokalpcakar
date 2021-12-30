using FluentValidation.Results;
using Icarus.API.Infrastructure.DistCache;
using Icarus.Model.User;
using Icarus.Service.User;
using Icarus.Service.ValidationRules;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Icarus.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IDistCache distCache;
        public UserController(IUserService _userService, IDistCache _distCache)
        {
            userService = _userService;
            distCache = _distCache;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginUser)
        {
            var model = userService.Login(loginUser);

            // login işlemi başarısızsa tekrardan login olması isteniyor
            if (!model.IsSuccess)
            {
                return View();
            }

            distCache.SetCache(loginUser);
            var cachedData = distCache.GetCurrentUser();

            // eğer cache'teki kullanıcı adminse kullanıcıyla alakalı işlemleri gerçekleştirebiliyor
            // admin değilse kullanıcı işlemlerini yapabileceği listeleme ekranına gidemiyor
            if (cachedData.IsAdmin)
            {
                return RedirectToAction("List", "User");
            }

            return RedirectToAction("List", "Product");
        }

        public IActionResult List()
        {
            var cachedData = distCache.GetCurrentUser();

            // eğer cache'teki kullanıcı admin değilse login ekranına yönlendiriliyor
            if (!cachedData.IsAdmin)
            {
                // CRUD işlemleri listeleme ekranı geldiğinde gerçekleşebileceğinden dolayı
                // bu if bloğu tüm kullanıcı işlemlerine erişimi engelliyor
                ViewBag.ExceptionMessage = "Kullanıcı işlemleri için yetkiniz bulunmamaktadır. Tekrar deneyiniz.";
                return View("Login");
            }

            return View(userService.GetUsers().List);
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(UserViewModel newProduct)
        {
            var model = userService.Insert(newProduct);

            if (!model.IsSuccess)
            {
                return View();
            }

            return RedirectToAction("List", "User");
        }

        public IActionResult UpdateUser(int id)
        {
            var model = userService.GetById(id);
            return View(model.Entity);
        }

        [HttpPost]
        public IActionResult UpdateUser(UserViewModel product)
        {
            // Kullanıcı güncellerken validasyon işlemine bakılıyor
            UserValidator productValidator = new UserValidator();
            ValidationResult results = productValidator.Validate(product);

            if (results.IsValid)
            {
                var model = userService.Update(product.Id, product);

                if (model.IsSuccess)
                {
                    return RedirectToAction("List", "User");
                }
            }
            else
            {
                // eğer validasyon işlemi geçerli değilse bu kısım devreye giriyor
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View();
        }

        public IActionResult DeleteUser(int id)
        {
            userService.Delete(id);
            return RedirectToAction("List", "User");
        }
    }
}
