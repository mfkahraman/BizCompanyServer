using BizCompany.API.DTOs;
using FluentValidation;

namespace BizCompany.API.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Ürün adı boş bırakılamaz.")
                .MinimumLength(3).WithMessage("Ürün adı en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Ürün adı en fazla 100 karakter olabilir.");
            RuleFor(x => x.Description)
                .MaximumLength(10000).WithMessage("Açıklama en fazla 10000 karakter olabilir.");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçersiz kategori ID'si.");
        }
    }
}
 