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
        public General<ListDeleteViewModel> GetProducts();
        public General<ListDeleteViewModel> SortProducts(string sortingParameter);
        public General<ListDeleteViewModel> FilterProducts(string filterByName);
        public General<ListDeleteViewModel> ProductPagination(int productByPage, int displayPageNo);
        public General<InsertProductViewModel> Insert(InsertProductViewModel newProduct);
        public General<UpdateProductViewModel> Update(int id, UpdateProductViewModel product);
        public General<ListDeleteViewModel> Delete(int id);
    }
}
