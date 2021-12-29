using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus.Model.User
{
    // Kullanıcıların giriş işleminde kullanılacak ViewModel
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Soyadı 6 iler 50 karakter arasında olmalıdır.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Şifre 3 ile 50 karakter arasında olmalıdır.")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}
