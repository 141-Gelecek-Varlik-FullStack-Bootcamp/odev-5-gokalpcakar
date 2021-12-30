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

        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Gösterilen Ad")]
        public string DisplayName { get; set; }

        [Display(Name = "Açıklama")] 
        public string Description { get; set; }

        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Display(Name = "Stok Adeti")]
        public int Stock { get; set; }
    }
}
