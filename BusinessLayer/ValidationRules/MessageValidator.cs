using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class MessageValidator : AbstractValidator<Message2>
    {
        public MessageValidator()
        {
            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Başlık boş olamaz")
                .MinimumLength(6).WithMessage("Başlık en az 6 karakter olmalıdır.");

            RuleFor(x => x.MessageDetails)
                .NotEmpty().WithMessage("İçerik boş olamaz")
                .MinimumLength(10).WithMessage("İçerik en az 10 karakter olmalıdır.");
        }
    }
}
