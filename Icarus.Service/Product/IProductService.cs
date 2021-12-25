using Icarus.Model;
using Icarus.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus.Service.Product
{
    // ProductService class'ında kullanılacak metotları burada tanımladık
    public interface IProductService
    {
        public General<ProductViewModel> GetProducts();
        public General<UpdateProductViewModel> GetById(int id);
        public General<ProductViewModel> SortProducts(string sortingParameter);
        public General<ProductViewModel> FilterProducts(string filterByName);
        public General<ProductViewModel> ProductPagination(int productByPage, int displayPageNo);
        public General<InsertProductViewModel> Insert(InsertProductViewModel newProduct);
        public General<UpdateProductViewModel> Update(UpdateProductViewModel product);
        public General<ProductViewModel> Delete(int id);
    }
}
