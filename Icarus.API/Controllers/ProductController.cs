using Icarus.API.Infrastructure;
using Icarus.Model;
using Icarus.Model.Product;
using Icarus.Service.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Icarus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // Burada servisi çağırıyoruz.
        private readonly IProductService productService;
        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }

        // Tüm ürünlerin listeleneceği metodun servis katmanından çağırıldığı kısım
        [HttpGet]
        public General<ListDeleteViewModel> GetProducts()
        {
            return productService.GetProducts();
        }

        // Ürün ekleme metodunun servis katmanından çağırıldığı kısım
        [HttpPost]
        public General<InsertProductViewModel> Insert([FromBody] InsertProductViewModel newProduct)
        {
            return productService.Insert(newProduct);
        }

        // Ürün güncelleme metodunun servis katmanından çağırıldığı kısım
        [HttpPut("{id}")]
        public General<UpdateProductViewModel> Update(int id, [FromBody] UpdateProductViewModel product)
        {
            return productService.Update(id, product);
        }

        // Ürün silme metodunun servis katmanından çağırıldığı kısım
        [HttpDelete("{id}")]
        public General<ListDeleteViewModel> Delete(int id)
        {
            return productService.Delete(id);
        }
    }
}
