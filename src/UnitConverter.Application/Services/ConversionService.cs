using FluentValidation;
using Microsoft.Extensions.Logging;
using UnitConverter.Application.DTOs;
using UnitConverter.Application.Interfaces;
using UnitConverter.Domain.Entities;

namespace UnitConverter.Application.Services;

public sealed class ConversionService
{
    private readonly IUnitRepository _units;
    private readonly IConversionHistoryRepository _history;
    private readonly IValidator<ConversionRequest> _validator;
    private readonly ILogger<ConversionService> _logger;

    public ConversionService(
        IUnitRepository units,
        IConversionHistoryRepository history,
        IValidator<ConversionRequest> validator,
        ILogger<ConversionService> logger)
    {
        _units = units;
        _history = history;
        _validator = validator;
        _logger = logger;
    }

    public ConversionResponse Convert(ConversionRequest request)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var fromUnit = _units.FindUnit(request.From)
            ?? throw new ArgumentException($"Unknown unit: '{request.From}'.", nameof(request));

        var toUnit = _units.FindUnit(request.To)
            ?? throw new ArgumentException($"Unknown unit: '{request.To}'.", nameof(request));

        if (fromUnit.Category != toUnit.Category)
            throw new ArgumentException(
                $"Cannot convert between categories: '{fromUnit.Category}' and '{toUnit.Category}'.");

        double siValue = (request.Value * fromUnit.Factor) + fromUnit.Offset;
        double result = (siValue - toUnit.Offset) / toUnit.Factor;
        result = Math.Round(result, 10);

        _logger.LogInformation(
            "Converted {Value} {From} -> {Result} {To} (category: {Category})",
            request.Value, fromUnit.Key, result, toUnit.Key, fromUnit.Category);

        _history.Add(new ConversionRecord
        {
            InputValue = request.Value,
            FromUnit = fromUnit.Key,
            OutputValue = result,
            ToUnit = toUnit.Key,
            Category = fromUnit.Category.ToString()
        });

        return new ConversionResponse(
            InputValue: request.Value,
            FromUnit: fromUnit.DisplayName,
            FromSymbol: fromUnit.Symbol,
            OutputValue: result,
            ToUnit: toUnit.DisplayName,
            ToSymbol: toUnit.Symbol,
            Category: fromUnit.Category.ToString(),
            Formula: BuildFormula(fromUnit, toUnit)
        );
    }

    private static string BuildFormula(UnitDefinition from, UnitDefinition to)
    {
        var siExpr = from.Offset == 0
            ? $"x * {from.Factor}"
            : $"(x * {from.Factor}) + {from.Offset}";

        var outExpr = to.Offset == 0 && to.Factor == 1
            ? "result"
            : to.Offset == 0
                ? $"result / {to.Factor}"
                : $"(result - {to.Offset}) / {to.Factor}";

        return $"{siExpr} -> {outExpr}";
    }
}
