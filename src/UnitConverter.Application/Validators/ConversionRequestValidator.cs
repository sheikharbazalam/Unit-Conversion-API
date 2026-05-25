using FluentValidation;
using UnitConverter.Application.DTOs;

namespace UnitConverter.Application.Validators;

public sealed class ConversionRequestValidator : AbstractValidator<ConversionRequest>
{
    public ConversionRequestValidator()
    {
        RuleFor(x => x.Value)
            .Must(v => !double.IsNaN(v) && !double.IsInfinity(v))
            .WithMessage("Value must be a finite number.");

        RuleFor(x => x.From)
            .NotEmpty()
            .WithMessage("'from' unit is required.")
            .MaximumLength(50);

        RuleFor(x => x.To)
            .NotEmpty()
            .WithMessage("'to' unit is required.")
            .MaximumLength(50);

        RuleFor(x => x)
            .Must(x => !string.Equals(x.From, x.To, StringComparison.OrdinalIgnoreCase))
            .WithMessage("'from' and 'to' units must be different.");
    }
}
