using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class BlogValidator : AbstractValidator<Blog>
    {
        public BlogValidator()
        {
            RuleFor(x => x.BlogTitle)
                .NotEmpty().WithMessage("Blog başlığı gereklidir.")
                .Length(3, 100).WithMessage("Blog başlığı 3 ile 100 karakter arasında olmalıdır.");

            RuleFor(x => x.BlogContent)
                .NotEmpty().WithMessage("Blog içeriği gereklidir.")
                .MinimumLength(20).WithMessage("Blog içeriği en az 20 karakter olmalıdır.");

            RuleFor(x => x.BlogImage)
                .NotEmpty().WithMessage("Blog resmi yüklenmelidir.");

            RuleFor(x => x.CategoryID)
                .GreaterThan(0).WithMessage("Kategori seçilmelidir.");

        }
    }
        
}
