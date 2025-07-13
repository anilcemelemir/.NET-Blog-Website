using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class WriterValidator : AbstractValidator<Writer>
    {
        public WriterValidator()
        {
            RuleFor(x => x.WriterName)
            .NotEmpty().WithMessage("Ad soyad boş olamaz")
            .MinimumLength(2).WithMessage("Ad soyad en az 2 karakter olmalıdır")
            .MaximumLength(50).WithMessage("Ad soyad en fazla 50 karakter olmalıdır");

            RuleFor(x => x.WriterMail)
                .NotEmpty().WithMessage("E-posta adresi boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi girin");

            RuleFor(x => x.WriterPassword)
                .NotEmpty().WithMessage("Şifre boş olamaz")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır")
                .Matches(@"[A-Z]+").WithMessage("Şifre en az bir büyük harf içermelidir")
                .Matches(@"[a-z]+").WithMessage("Şifre en az bir küçük harf içermelidir")
                .Matches(@"[0-9]+").WithMessage("Şifre en az bir rakam içermelidir")
                .Matches(@"[\W]+").WithMessage("Şifre en az bir özel karakter içermelidir");

        }
    }
}
