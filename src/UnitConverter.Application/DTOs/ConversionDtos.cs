namespace UnitConverter.Application.DTOs;

public sealed record ConversionRequest(
    double Value,
    string From,
    string To
);

public sealed record ConversionResponse(
    double InputValue,
    string FromUnit,
    string FromSymbol,
    double OutputValue,
    string ToUnit,
    string ToSymbol,
    string Category,
    string Formula
);

public sealed record UnitSummaryDto(
    string Key,
    string DisplayName,
    string Symbol,
    string Category,
    IReadOnlyList<string> Aliases
);

public sealed record ConversionHistoryDto(
    Guid Id,
    double InputValue,
    string FromUnit,
    double OutputValue,
    string ToUnit,
    string Category,
    DateTimeOffset ConvertedAt
);
