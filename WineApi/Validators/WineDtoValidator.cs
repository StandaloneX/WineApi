using FluentValidation;
using WineApi.Models;

namespace WineApi.Validators
{
    public class WineDtoValidator : AbstractValidator<WineDto>
    {
        public WineDtoValidator()
        {
            RuleFor(w => w.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(2, 50).WithMessage("Title must be between 2 and 50 characters.");

            RuleFor(w => w.Type)
                .NotEmpty().WithMessage("Type is required.")
                .Length(2, 50).WithMessage("Type must be between 2 and 50 characters.");

            RuleFor(w => w.Brand)
                .NotEmpty().WithMessage("Brand is required.")
                .Length(2, 50).WithMessage("Brand must be between 2 and 50 characters.");

            RuleFor(w => w.Year)
                .GreaterThan(0).WithMessage("Year must be greater than 0.");
        }
    }
}
