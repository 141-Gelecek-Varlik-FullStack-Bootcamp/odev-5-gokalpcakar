using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus.Model.Product
{
    // Ürünlerin güncellemesinde kullanılacak ViewModel
    public class UpdateProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori bilgisi girilmelidir.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kullanıcı bilgisi girilmelidir.")]
        [Display(Name = "Kullanıcı")]
        public int Iuser { get; set; }

        [Required(ErrorMessage = "Ürün adı boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "Ürün adı 50 karakterden fazla olmamalıdır.")]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gösterim adı boş bırakılamaz!")]
        [StringLength(50, ErrorMessage = "Ürün gösterim adı 50 karakterden fazla olmamalıdır.")]
        [Display(Name = "Gösterim Adı")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Açıklama alanı boş bırakılamaz.")]
        [StringLength(250, ErrorMessage = "Açıklama 250 karakterden fazla olmamalıdır.")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fiyat girmek zorunludur.")]
        [Display(Name = "Ürün Fiyatı")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stok adeti boş bırakılamaz!")]
        [Display(Name = "Stok Adeti")]
        public int Stock { get; set; }
    }
}
