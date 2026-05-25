using UnitConverter.Domain.Entities;
using UnitConverter.Domain.Enums;

namespace UnitConverter.Infrastructure.Data;

internal static class UnitDefinitionSeed
{
    public static readonly IReadOnlyList<UnitDefinition> All = BuildAll();

    private static List<UnitDefinition> BuildAll() =>
    [
        //LENGTH (SI base: meter)
        new() { Key="meter",      DisplayName="Meter",        Symbol="m",    Category=ConversionCategory.Length, Factor=1.0,          Aliases=["metre","m"] },
        new() { Key="kilometer",  DisplayName="Kilometer",    Symbol="km",   Category=ConversionCategory.Length, Factor=1000.0,       Aliases=["kilometre","km"] },
        new() { Key="centimeter", DisplayName="Centimeter",   Symbol="cm",   Category=ConversionCategory.Length, Factor=0.01,         Aliases=["centimetre","cm"] },
        new() { Key="millimeter", DisplayName="Millimeter",   Symbol="mm",   Category=ConversionCategory.Length, Factor=0.001,        Aliases=["millimetre","mm"] },
        new() { Key="mile",       DisplayName="Mile",         Symbol="mi",   Category=ConversionCategory.Length, Factor=1609.344,     Aliases=["miles","mi"] },
        new() { Key="yard",       DisplayName="Yard",         Symbol="yd",   Category=ConversionCategory.Length, Factor=0.9144,       Aliases=["yards","yd"] },
        new() { Key="foot",       DisplayName="Foot",         Symbol="ft",   Category=ConversionCategory.Length, Factor=0.3048,       Aliases=["feet","ft"] },
        new() { Key="inch",       DisplayName="Inch",         Symbol="in",   Category=ConversionCategory.Length, Factor=0.0254,       Aliases=["inches","in"] },
        new() { Key="nautical_mile", DisplayName="Nautical Mile", Symbol="nmi", Category=ConversionCategory.Length, Factor=1852.0,   Aliases=["nauticalmile","nmi"] },

        // TEMPERATURE (SI base: kelvin)
        new() { Key="kelvin",     DisplayName="Kelvin",       Symbol="K",    Category=ConversionCategory.Temperature, Factor=1.0,    Offset=0.0,       Aliases=["k"] },
        new() { Key="celsius",    DisplayName="Celsius",      Symbol="C",    Category=ConversionCategory.Temperature, Factor=1.0,    Offset=273.15,    Aliases=["centigrade","c"] },
        // offset = (32 * 5/9) subtracted from 273.15 - expressed as exact fraction and that why we added offset value 2298.35/9.0 , to avoid floating point accumulation error.
        new() { Key="fahrenheit", DisplayName="Fahrenheit",   Symbol="F",    Category=ConversionCategory.Temperature, Factor=5.0/9.0, Offset=2298.35/9.0, Aliases=["f"] },
        new() { Key="rankine",    DisplayName="Rankine",      Symbol="R",    Category=ConversionCategory.Temperature, Factor=5.0/9.0, Offset=0.0,      Aliases=["r"] },

        // WEIGHT (SI base: kilogram)
        new() { Key="kilogram",   DisplayName="Kilogram",     Symbol="kg",   Category=ConversionCategory.Weight, Factor=1.0,         Aliases=["kilograms","kg"] },
        new() { Key="gram",       DisplayName="Gram",         Symbol="g",    Category=ConversionCategory.Weight, Factor=0.001,       Aliases=["grams","g"] },
        new() { Key="milligram",  DisplayName="Milligram",    Symbol="mg",   Category=ConversionCategory.Weight, Factor=1e-6,        Aliases=["milligrams","mg"] },
        new() { Key="tonne",      DisplayName="Metric Tonne", Symbol="t",    Category=ConversionCategory.Weight, Factor=1000.0,      Aliases=["metric ton","t"] },
        new() { Key="pound",      DisplayName="Pound",        Symbol="lb",   Category=ConversionCategory.Weight, Factor=0.45359237,  Aliases=["pounds","lb","lbs"] },
        new() { Key="ounce",      DisplayName="Ounce",        Symbol="oz",   Category=ConversionCategory.Weight, Factor=0.02834952,  Aliases=["ounces","oz"] },
        new() { Key="stone",      DisplayName="Stone",        Symbol="st",   Category=ConversionCategory.Weight, Factor=6.35029318,  Aliases=["stones","st"] },

        // VOLUME (SI base: litre)
        new() { Key="litre",      DisplayName="Litre",        Symbol="L",    Category=ConversionCategory.Volume, Factor=1.0,         Aliases=["liter","l"] },
        new() { Key="millilitre", DisplayName="Millilitre",   Symbol="mL",   Category=ConversionCategory.Volume, Factor=0.001,       Aliases=["milliliter","ml"] },
        new() { Key="cubic_meter",DisplayName="Cubic Meter",  Symbol="m3",   Category=ConversionCategory.Volume, Factor=1000.0,      Aliases=["cubicmeter","m3"] },
        new() { Key="gallon_us",  DisplayName="Gallon (US)",  Symbol="gal",  Category=ConversionCategory.Volume, Factor=3.785411784, Aliases=["gallon","gal"] },
        new() { Key="gallon_uk",  DisplayName="Gallon (UK)",  Symbol="ukgal",Category=ConversionCategory.Volume, Factor=4.54609,     Aliases=["imperial gallon","ukgal"] },
        new() { Key="pint",       DisplayName="Pint (US)",    Symbol="pt",   Category=ConversionCategory.Volume, Factor=0.473176473, Aliases=["pints","pt"] },
        new() { Key="cup",        DisplayName="Cup",          Symbol="cup",  Category=ConversionCategory.Volume, Factor=0.2365882,   Aliases=["cups"] },
        new() { Key="tablespoon", DisplayName="Tablespoon",   Symbol="tbsp", Category=ConversionCategory.Volume, Factor=0.01478676,  Aliases=["tbsp"] },
        new() { Key="teaspoon",   DisplayName="Teaspoon",     Symbol="tsp",  Category=ConversionCategory.Volume, Factor=0.00492892,  Aliases=["tsp"] },

        // SPEED (SI base: m/s)
        new() { Key="mps",        DisplayName="Meters/second",   Symbol="m/s",  Category=ConversionCategory.Speed, Factor=1.0,       Aliases=["m/s"] },
        new() { Key="kph",        DisplayName="Kilometers/hour", Symbol="km/h", Category=ConversionCategory.Speed, Factor=1.0/3.6,   Aliases=["km/h","kmh"] },
        new() { Key="mph",        DisplayName="Miles/hour",      Symbol="mph",  Category=ConversionCategory.Speed, Factor=0.44704,   Aliases=["miles per hour"] },
        new() { Key="knot",       DisplayName="Knot",            Symbol="kn",   Category=ConversionCategory.Speed, Factor=0.514444,  Aliases=["knots","kn"] },
        new() { Key="mach",       DisplayName="Mach",            Symbol="Ma",   Category=ConversionCategory.Speed, Factor=343.0,     Aliases=["ma"] },

        // AREA (SI base: m2)
        new() { Key="sqmeter",    DisplayName="Square Meter",    Symbol="m2",   Category=ConversionCategory.Area, Factor=1.0,        Aliases=["m2","square meter"] },
        new() { Key="sqkilometer",DisplayName="Square Kilometer",Symbol="km2",  Category=ConversionCategory.Area, Factor=1e6,        Aliases=["km2"] },
        new() { Key="sqmile",     DisplayName="Square Mile",     Symbol="mi2",  Category=ConversionCategory.Area, Factor=2589988.11, Aliases=["mi2","square mile"] },
        new() { Key="sqfoot",     DisplayName="Square Foot",     Symbol="ft2",  Category=ConversionCategory.Area, Factor=0.09290304, Aliases=["ft2","square foot"] },
        new() { Key="sqinch",     DisplayName="Square Inch",     Symbol="in2",  Category=ConversionCategory.Area, Factor=0.00064516, Aliases=["in2","square inch"] },
        new() { Key="hectare",    DisplayName="Hectare",         Symbol="ha",   Category=ConversionCategory.Area, Factor=10000.0,    Aliases=["ha"] },
        new() { Key="acre",       DisplayName="Acre",            Symbol="ac",   Category=ConversionCategory.Area, Factor=4046.856422,Aliases=["acres","ac"] },
    ];
}
