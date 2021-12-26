using Icarus.DB.Entities.DataContext;
using Icarus.Model.Product;
using Icarus.Model.User;
using Icarus.Service.Product;
using Icarus.Service.User;
using Icarus.Service.ValidationRules;
using Icarus.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using System.Net.Http.Json;
using System.Net.Http;

namespace Icarus.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;
        private readonly IUserService userService;

        public HomeController(ILogger<HomeController> logger, IProductService _productService, IUserService _userService)
        {
            _logger = logger;
            productService = _productService;
            userService = _userService;
        }

        public IActionResult Index()
        {
            return View(productService.GetProducts().List);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginUser)
        {
            var model = userService.Login(loginUser);

            if (!model.IsSuccess)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult InsertProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertProduct(InsertProductViewModel newProduct)
        {
            var model = productService.Insert(newProduct);

            if (!model.IsSuccess)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult UpdateProduct(int id)
        {
            var model = productService.GetById(id);
            return View(model.Entity);
        }

        [HttpPost]
        public IActionResult UpdateProduct(UpdateProductViewModel product)
        {
            ProductValidator productValidator = new ProductValidator();
            ValidationResult results = productValidator.Validate(product);

            if (results.IsValid)
            {
                var model = productService.Update(product);

                if (model.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View();
        }

        public IActionResult DeleteProduct(int id)
        {
            var model = productService.Delete(id);

            if (!model.IsSuccess)
            {
                return RedirectToAction("Login", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
