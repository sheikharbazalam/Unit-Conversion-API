using UnitConverter.Domain.Entities;

namespace UnitConverter.Application.Interfaces;

public interface IConversionHistoryRepository
{
    void Add(ConversionRecord record);
    IReadOnlyList<ConversionRecord> GetRecent(int limit = 20);
}
