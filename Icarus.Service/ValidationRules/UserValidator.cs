using FluentValidation;
using Icarus.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus.Service.ValidationRules
{
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Adınızı boş geçemezsiniz");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyadınızı boş bırakamazsınız");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adını boş bırakamazsınız");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta'yı boş bırakamazsınız");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifreyi boş bırakamazsınız");
        }
    }
}
