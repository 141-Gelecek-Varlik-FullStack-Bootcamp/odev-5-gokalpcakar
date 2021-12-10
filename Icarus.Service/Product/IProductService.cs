﻿using Icarus.Model;
using Icarus.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus.Service.Product
{
    public interface IProductService
    {
        public General<ProductViewModel> GetProducts();
        public General<ProductViewModel> Insert(ProductViewModel newProduct);
        public General<ProductViewModel> Update(int id, ProductViewModel product);
        public General<ProductViewModel> Delete(int id);
    }
}