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
using System.Text.Json;
using Icarus.Model;
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
            //return View(productService.GetProducts().List);
            return View();
        }
        //public IActionResult Index()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var uri = new Uri("https://localhost:5001/api/Product");
        //        var responseTask = client.GetAsync(string.Format("https://localhost:5001/api/Product"));
        //        responseTask.Wait();
        //        var response = responseTask.Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var readTask = response.Content.ReadAsStringAsync();
        //            readTask.Wait();
        //            var result = JsonSerializer.Deserialize<General<List<ProductViewModel>>>(readTask.Result);
        //            ViewBag.ProductList = result.Entity;
        //            return View();
        //        }
        //        else
        //        {
        //            ViewBag.StatusCode = response.StatusCode;
        //        }
        //    }

        //    return View();
        //}

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
