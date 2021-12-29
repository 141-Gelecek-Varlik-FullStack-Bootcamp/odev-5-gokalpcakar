using FluentValidation.Results;
using Icarus.Model.Product;
using Icarus.Service.Product;
using Icarus.Service.ValidationRules;
using Microsoft.AspNetCore.Mvc;

namespace Icarus.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }
        public IActionResult List()
        {
            return View(productService.GetProducts().List);
        }
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(InsertProductViewModel newProduct)
        {
            var model = productService.Insert(newProduct);

            if (!model.IsSuccess)
            {
                return View();
            }

            return RedirectToAction("List", "Product");
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
                    return RedirectToAction("List", "Product");
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
                return RedirectToAction("Login", "User");
            }
            return RedirectToAction("List", "Product");
        }
    }
}
