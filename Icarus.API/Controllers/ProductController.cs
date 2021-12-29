using Icarus.API.Infrastructure;
using Icarus.DB.Entities;
using Icarus.DB.Entities.DataContext;
using Icarus.Model;
using Icarus.Model.Product;
using Icarus.Service.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Icarus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // LoginFilter tüm controller'a uygulandı
    //[ServiceFilter(typeof(LoginFilter))]
    public class ProductController : BaseController
    {
        // Burada servisi çağırıyoruz.
        private readonly IProductService productService;
        public ProductController(IProductService _productService, IDistributedCache _distributedCache) : base(_distributedCache) 
        {
            productService = _productService;
        }

        // Tüm ürünlerin listeleneceği metodun servis katmanından çağırıldığı kısım
        [HttpGet]
        public General<ProductViewModel> GetProducts()
        {
            return productService.GetProducts();
        }
        //[HttpGet]
        //public IActionResult GetProducts()
        //{
        //    var result = JsonSerializer.Serialize(productService.GetProducts());
        //    return Ok(result);
        //}

        // Sıralanmış ürünlerin listeleneceği metodun servis katmanından çağırıldığı kısım
        [HttpGet]
        [Route("SortBy")]
        public General<ProductViewModel> SortProducts([FromQuery] string sortingParameter)
        {
            return productService.SortProducts(sortingParameter);
        }

        // Filtrelenmiş ürünlerin listeleneceği metodun servis katmanından çağırıldığı kısım
        [HttpGet]
        [Route("FilterBy")]
        public General<ProductViewModel> FilterProducts([FromQuery] string filterByName)
        {
            return productService.FilterProducts(filterByName);
        }

        // Ürünlerin sayfalanacağı metodun servis katmanından çağırıldığı kısım
        [HttpGet]
        [Route("Pagination")]
        public General<ProductViewModel> ProductPagination([FromQuery] int productByPage, [FromQuery] int displayPageNo)
        {
            return productService.ProductPagination(productByPage, displayPageNo);
        }

        // Ürün ekleme metodunun servis katmanından çağırıldığı kısım
        [HttpPost]
        // Eğer kullanıcı adminse silme işlemi gerçekleştirilecek
        [ServiceFilter(typeof(AuthFilter))]
        public General<InsertProductViewModel> Insert([FromBody] InsertProductViewModel newProduct)
        {
            // ekleyen kullanıcı şuanda giriş yapmış kullanıcı olarak atanıyor
            newProduct.Iuser = CurrentUser.Id;
            return productService.Insert(newProduct);
        }

        // Ürün güncelleme metodunun servis katmanından çağırıldığı kısım
        [HttpPut("{id}")]
        // Eğer kullanıcı adminse silme işlemi gerçekleştirilecek
        [ServiceFilter(typeof(AuthFilter))]
        public General<UpdateProductViewModel> Update([FromBody] UpdateProductViewModel product)
        {
            return productService.Update(product);
        }

        // Ürün silme metodunun servis katmanından çağırıldığı kısım
        [HttpDelete("{id}")]
        // Eğer kullanıcı adminse silme işlemi gerçekleştirilecek
        [ServiceFilter(typeof(AuthFilter))]
        public General<ProductViewModel> Delete(int id)
        {
            return productService.Delete(id);
        }
    }
}
