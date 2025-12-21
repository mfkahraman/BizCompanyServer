using BizCompany.API.DTOs;
using FluentValidation;

namespace BizCompany.API.Validators
{
    public class CategoryValidator : AbstractValidator<ProductCategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name cannot be empty.")
                .MinimumLength(3).WithMessage("Category name must be at least 3 characters.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");
        }
    }
}
