using FluentAssertions;
using UnitConverter.Domain.Enums;
using UnitConverter.Infrastructure.Repositories;
using Xunit;

namespace UnitConverter.Tests.Services;

public sealed class InMemoryUnitRepositoryTests
{
    private readonly InMemoryUnitRepository _sut = new();

    [Theory]
    [InlineData("meter")]
    [InlineData("metre")]
    [InlineData("m")]
    [InlineData("METER")]
    public void FindUnit_ByKeyOrAlias_CaseInsensitive(string input)
    {
        var unit = _sut.FindUnit(input);
        unit.Should().NotBeNull();
        unit!.Key.Should().Be("meter");
    }

    [Fact]
    public void FindUnit_UnknownKey_ReturnsNull()
    {
        _sut.FindUnit("parsec").Should().BeNull();
    }

    [Fact]
    public void GetByCategory_ReturnsOnlyMatchingCategory()
    {
        var temps = _sut.GetByCategory(ConversionCategory.Temperature);
        temps.Should().HaveCountGreaterThanOrEqualTo(4);
        temps.Should().OnlyContain(u => u.Category == ConversionCategory.Temperature);
    }

    [Fact]
    public void GetCategories_ReturnsAllExpectedCategories()
    {
        var cats = _sut.GetCategories();
        cats.Should().Contain(ConversionCategory.Length);
        cats.Should().Contain(ConversionCategory.Temperature);
        cats.Should().Contain(ConversionCategory.Weight);
    }

    [Fact]
    public void GetAll_ReturnsNonEmptyList()
    {
        _sut.GetAll().Should().HaveCountGreaterThan(10);
    }
}
