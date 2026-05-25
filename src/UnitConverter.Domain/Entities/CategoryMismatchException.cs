namespace UnitConverter.Domain.Entities;

public sealed class CategoryMismatchException : Exception
{
    public string FromUnit { get; }
    public string FromCategory { get; }
    public IReadOnlyList<string> CanConvertTo { get; }
    public CategoryMismatchException(string fromUnit, string fromCategory, IReadOnlyList<string> canConvertTo)
        : base($"Cannot convert from '{fromUnit}' in category '{fromCategory}' See Suggestions for this.")
    {
        FromUnit = fromUnit;
        FromCategory = fromCategory;
        CanConvertTo = canConvertTo;
    }
}