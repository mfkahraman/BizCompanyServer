using BizCompany.API.DTOs;
using FluentValidation;

namespace BizCompany.API.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name cannot be empty.")
                .MinimumLength(3).WithMessage("Product name must be at least 3 characters.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(10000).WithMessage("Description cannot exceed 10000 characters.");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Invalid category ID.");
        }
    }
} 