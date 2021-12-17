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
    // LoginFilter tüm controller'a uygulandı
    [ServiceFilter(typeof(LoginFilter))]
    public class ProductController : BaseController
    {
        // Burada servisi çağırıyoruz.
        private readonly IProductService productService;
        public ProductController(IProductService _productService, IMemoryCache _memoryCache) : base(_memoryCache)
        {
            productService = _productService;
        }

        // Tüm ürünlerin listeleneceği metodun servis katmanından çağırıldığı kısım
        [HttpGet]
        public General<ListDeleteViewModel> GetProducts()
        {
            return productService.GetProducts();
        }

        // Sıralanmış ürünlerin listeleneceği metodun servis katmanından çağırıldığı kısım
        [HttpGet]
        [Route("SortBy")]
        public General<ListDeleteViewModel> SortProducts([FromQuery] string sortingParameter)
        {
            return productService.SortProducts(sortingParameter);
        }

        // Filtrelenmiş ürünlerin listeleneceği metodun servis katmanından çağırıldığı kısım
        [HttpGet]
        [Route("FilterBy")]
        public General<ListDeleteViewModel> FilterProducts([FromQuery] string filterByName)
        {
            return productService.FilterProducts(filterByName);
        }

        // Ürünlerin sayfalanacağı metodun servis katmanından çağırıldığı kısım
        [HttpGet]
        [Route("Pagination")]
        public General<ListDeleteViewModel> ProductPagination([FromQuery] int productByPage, [FromQuery] int displayPageNo)
        {
            return productService.ProductPagination(productByPage, displayPageNo);
        }

        // Ürün ekleme metodunun servis katmanından çağırıldığı kısım
        [HttpPost]
        public General<InsertProductViewModel> Insert([FromBody] InsertProductViewModel newProduct)
        {
            // ekleyen kullanıcı şuanda giriş yapmış kullanıcı olarak atanıyor
            newProduct.Iuser = CurrentUser.Id;
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
