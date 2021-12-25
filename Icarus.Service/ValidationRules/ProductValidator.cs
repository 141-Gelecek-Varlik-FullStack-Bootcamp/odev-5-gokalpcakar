using Icarus.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Icarus.Service.ValidationRules
{
    public class ProductValidator : AbstractValidator<UpdateProductViewModel>
    {
        public ProductValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategoriyi boş geçemezsiniz");
            RuleFor(x => x.Iuser).NotEmpty().WithMessage("Kullanıcıyı boş geçemezsiniz");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ürün adını boş geçemezsiniz");
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage("Gösterin adını boş bırakamazsınız");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklamayı boş bırakamazsınız");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyatı boş bırakamazsınız");
            RuleFor(x => x.Stock).NotEmpty().WithMessage("Stok adetini boş bırakamazsınız");
        }
    }
}
