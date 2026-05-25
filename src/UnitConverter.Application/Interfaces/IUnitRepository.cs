using UnitConverter.Domain.Entities;
using UnitConverter.Domain.Enums;

namespace UnitConverter.Application.Interfaces;

public interface IUnitRepository
{
    UnitDefinition? FindUnit(string keyOrAlias);
    IReadOnlyList<UnitDefinition> GetByCategory(ConversionCategory category);
    IReadOnlyList<ConversionCategory> GetCategories();
    IReadOnlyList<UnitDefinition> GetAll();
}
