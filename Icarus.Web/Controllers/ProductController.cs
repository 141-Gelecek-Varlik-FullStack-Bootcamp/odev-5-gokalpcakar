using FluentValidation.Results;
using Icarus.API.Infrastructure.DistCache;
using Icarus.Model.Product;
using Icarus.Service.Product;
using Icarus.Service.ValidationRules;
using Microsoft.AspNetCore.Mvc;

namespace Icarus.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IDistCache distCache;
        public ProductController(IProductService _productService, IDistCache _distCache)
        {
            productService = _productService;
            distCache = _distCache;
        }
        public IActionResult List()
        {
            // ürünler listeleniyor ve o view'a cache'te kullanıcının admin bilgisi gönderiliyor
            var cachedData = distCache.GetCurrentUser();
            ViewBag.IsAdmin = cachedData.IsAdmin;
            return View(productService.GetProducts().List);
        }

        // Ürün ekleme ekranına gidilyor
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(InsertProductViewModel newProduct)
        {
            var model = productService.Insert(newProduct);

            // eğer ürün ekleme işlemi başarılıysa ürünler listeleniyor
            // değilse ürünler tekrardan girilsin diye ürün ekleme sayfası açılıyor
            if (!model.IsSuccess)
            {
                return View();
            }

            return RedirectToAction("List", "Product");
        }

        public IActionResult UpdateProduct(int id)
        {
            // güncelleme view'ına ilgili ürün ViewModel'i gönderiliyor
            var model = productService.GetById(id);
            return View(model.Entity);
        }

        [HttpPost]
        public IActionResult UpdateProduct(UpdateProductViewModel product)
        {
            // View'da validasyon ile ilgili işlem yapabilmek için bu kısmı kullanıyoruz
            ProductValidator productValidator = new ProductValidator();
            ValidationResult results = productValidator.Validate(product);

            if (results.IsValid)
            {
                var model = productService.Update(product);

                if (model.IsSuccess)
                {
                    // güncelleme işlemi başarılıysa ürünler listeleniyor
                    return RedirectToAction("List", "Product");
                }
            }
            else
            {
                // validasyon işlemi geçerli değilse bu kısım devreye giriyor
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View();
        }

        public IActionResult DeleteProduct(int id)
        {
            productService.Delete(id);
            return RedirectToAction("List", "Product");
        }
    }
}
