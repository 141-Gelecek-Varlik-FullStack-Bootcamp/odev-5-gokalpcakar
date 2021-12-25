using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus.Model.Product
{
    // Genel ürün işlemlerinde kullanılacak ViewModel
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
