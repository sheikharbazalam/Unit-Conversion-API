namespace UnitConverter.Domain.Entities;

public sealed class ConversionRecord
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public double InputValue { get; init; }
    public string FromUnit { get; init; } = string.Empty;
    public double OutputValue { get; init; }
    public string ToUnit { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public DateTimeOffset ConvertedAt { get; init; } = DateTimeOffset.UtcNow;
}
