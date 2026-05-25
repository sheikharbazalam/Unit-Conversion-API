using UnitConverter.Domain.Enums;

namespace UnitConverter.Domain.Entities;

public sealed class UnitDefinition
{
    public string Key { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public IReadOnlyList<string> Aliases { get; init; } = [];
    public ConversionCategory Category { get; init; }
    public double Factor { get; init; } = 1.0;
    public double Offset { get; init; } = 0.0;
    public string Symbol {get; init;} = string.Empty;
}
