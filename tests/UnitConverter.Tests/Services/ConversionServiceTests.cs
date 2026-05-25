using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using UnitConverter.Application.DTOs;
using UnitConverter.Application.Interfaces;
using UnitConverter.Application.Services;
using UnitConverter.Application.Validators;
using UnitConverter.Domain.Entities;
using UnitConverter.Infrastructure.Repositories;
using Xunit;

namespace UnitConverter.Tests.Services;

public sealed class ConversionServiceTests
{
    private readonly ConversionService _sut;
    private readonly IConversionHistoryRepository _history = Substitute.For<IConversionHistoryRepository>();

    public ConversionServiceTests()
    {
        var repo = new InMemoryUnitRepository();
        var validator = new ConversionRequestValidator();
        _sut = new ConversionService(repo, _history, validator, NullLogger<ConversionService>.Instance);
    }

    [Theory]
    [InlineData(1.0, "meter", "foot", 3.280839895)]
    [InlineData(1.0, "kilometer", "mile", 0.6213711922)]
    [InlineData(1.0, "inch", "centimeter", 2.54)]
    [InlineData(100.0, "centimeter", "meter", 1.0)]
    public void Convert_Length_ReturnsExpectedResult(
        double input, string from, string to, double expected)
    {
        var result = _sut.Convert(new ConversionRequest(input, from, to));
        result.OutputValue.Should().BeApproximately(expected, precision: 1e-6);
    }

    [Theory]
    [InlineData(0.0, "celsius", "fahrenheit", 32.0)]
    [InlineData(100.0, "celsius", "fahrenheit", 212.0)]
    [InlineData(0.0, "celsius", "kelvin", 273.15)]
    [InlineData(32.0, "fahrenheit", "celsius", 0.0)]
    [InlineData(-40.0, "celsius", "fahrenheit", -40.0)]
    public void Convert_Temperature_ReturnsExpectedResult(
        double input, string from, string to, double expected)
    {
        var result = _sut.Convert(new ConversionRequest(input, from, to));
        result.OutputValue.Should().BeApproximately(expected, precision: 1e-4);
    }

    [Theory]
    [InlineData(1.0, "kilogram", "pound", 2.2046226218)]
    [InlineData(1.0, "pound", "kilogram", 0.45359237)]
    [InlineData(1.0, "stone", "kilogram", 6.35029318)]
    public void Convert_Weight_ReturnsExpectedResult(
        double input, string from, string to, double expected)
    {
        var result = _sut.Convert(new ConversionRequest(input, from, to));
        result.OutputValue.Should().BeApproximately(expected, precision: 1e-6);
    }

    [Theory]
    [InlineData("m", "ft")]
    [InlineData("metre", "foot")]
    [InlineData("kg", "lb")]
    public void Convert_AcceptsAliases(string from, string to)
    {
        var act = () => _sut.Convert(new ConversionRequest(1.0, from, to));
        act.Should().NotThrow();
    }

    [Fact]
    public void Convert_DifferentCategories_ThrowsArgumentException()
    {
        var act = () => _sut.Convert(new ConversionRequest(1.0, "meter", "kilogram"));
        act.Should().Throw<ArgumentException>().WithMessage("*categories*");
    }

    [Fact]
    public void Convert_UnknownFromUnit_ThrowsArgumentException()
    {
        var act = () => _sut.Convert(new ConversionRequest(1.0, "lightsaber", "meter"));
        act.Should().Throw<ArgumentException>().WithMessage("*lightsaber*");
    }

    [Fact]
    public void Convert_UnknownToUnit_ThrowsArgumentException()
    {
        var act = () => _sut.Convert(new ConversionRequest(1.0, "meter", "warp9"));
        act.Should().Throw<ArgumentException>().WithMessage("*warp9*");
    }

    [Fact]
    public void Convert_EmptyFromUnit_ThrowsValidationException()
    {
        var act = () => _sut.Convert(new ConversionRequest(1.0, "", "meter"));
        act.Should().Throw<ValidationException>();
    }

    [Fact]
    public void Convert_SameFromAndTo_ThrowsValidationException()
    {
        var act = () => _sut.Convert(new ConversionRequest(1.0, "meter", "meter"));
        act.Should().Throw<ValidationException>();
    }

    [Fact]
    public void Convert_NaNValue_ThrowsValidationException()
    {
        var act = () => _sut.Convert(new ConversionRequest(double.NaN, "meter", "foot"));
        act.Should().Throw<ValidationException>();
    }

    [Fact]
    public void Convert_OnSuccess_PersistsHistoryRecord()
    {
        _sut.Convert(new ConversionRequest(1.0, "meter", "foot"));
        _history.Received(1).Add(Arg.Is<ConversionRecord>(r =>
            r.FromUnit == "meter" && r.ToUnit == "foot"));
    }

    [Fact]
    public void Convert_ResponseContainsExpectedFields()
    {
        var result = _sut.Convert(new ConversionRequest(1.0, "meter", "foot"));
        result.Should().NotBeNull();
        result.Category.Should().Be("Length");
        result.FromUnit.Should().Be("Meter");
        result.ToUnit.Should().Be("Foot");
        result.FromSymbol.Should().Be("m");
        result.ToSymbol.Should().Be("ft");
        result.Formula.Should().NotBeNullOrEmpty();
    }
}
