using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.CommentTitle).NotEmpty().WithMessage("Başlık boş geçilemez");
            RuleFor(x => x.CommmentContent).NotEmpty().WithMessage("Yorum boş geçilemez");
            RuleFor(x => x.CommmentContent).MaximumLength(500).WithMessage("Yorum en fazla 500 karakter olabilir");
            RuleFor(x => x.CommmentContent).MinimumLength(10).WithMessage("Yorum en az 10 karakter olabilir");
        }
    }
}
