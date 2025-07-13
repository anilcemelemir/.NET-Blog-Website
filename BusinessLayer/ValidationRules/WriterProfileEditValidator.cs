using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class WriterProfileEditValidator : AbstractValidator<Writer>
    {
        public WriterProfileEditValidator()
        {
            RuleFor(x => x.WriterName)
            .NotEmpty().WithMessage("Ad soyad boş olamaz")
            .MinimumLength(2).WithMessage("Ad soyad en az 2 karakter olmalıdır")
            .MaximumLength(50).WithMessage("Ad soyad en fazla 50 karakter olmalıdır");

            RuleFor(x => x.WriterAbout)
            .NotEmpty().WithMessage("Hakkında kısmı boş olamaz.")
            .MinimumLength(10).WithMessage("Hakkında kısmı en az 10 karakter olmalıdır")
            .MaximumLength(200).WithMessage("Hakkında kısmı en fazla 200 karakter olmalıdır");

        }
    }
}
