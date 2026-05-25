using UnitConverter.Application.DTOs;
using UnitConverter.Application.Interfaces;

namespace UnitConverter.Application.Services;

public sealed class HistoryService
{
    private readonly IConversionHistoryRepository _repo;

    public HistoryService(IConversionHistoryRepository repo) => _repo = repo;

    public IReadOnlyList<ConversionHistoryDto> GetRecent(int limit = 20)
    {
        if (limit is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 100.");

        return _repo.GetRecent(limit)
                    .Select(r => new ConversionHistoryDto(
                        r.Id,
                        r.InputValue,
                        r.FromUnit,
                        r.OutputValue,
                        r.ToUnit,
                        r.Category,
                        r.ConvertedAt))
                    .ToList();
    }
}
