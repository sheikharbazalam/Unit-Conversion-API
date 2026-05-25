A RESTful ASP.NET Core 9 Web API converting numerical values between units of measurement. Its built on clean architecture to give user seamless solution on this project and it also testable, maintable and easily to extend.

## Features

- 6 conversion categories : Length, Temperature, Weight, Volume, speed and Area.
- 40+ units with alias support (e.g. `m`, `metre`, `meter` all work)
- Batch conversion endpoint (up to 20 at once)
- Conversion history endpoint
- Unit catalogue endpoint for discovery
- RFC 7807 Problem Details error responses
- Swagger UI at `/`
- Rate limiting (100 requests/minute)- so user cant send more that given limit request per minute per IP address using a fixed window limiter, which protect the server from unnecessary and excessive load.
- Structured logging via Serilog
- Clean Architecture (Domain / Application / Infrastructure / API)
- Unit tests with xUnit, FluentAssertions, NSubstitute - All test cases checked which help to understand the scenarios.

## Project Structure

src/
 UnitConverter.Domain/         # Entities and Enums - no dependencies
 UnitConverter.Application/    # Business Logic, interfaces, DTOs, validators
 UnitConverter.Infrastructure/ #in Memory repositories and unit seed data
 UnitConverter.API/            #Controllers, middleware, Program.cs
tests/
 UnitConverter.Tests/ #31 Unit tests

How To Run Locally
## Prerequisites
. .NET 9 SDK — https://dotnet.microsoft.com/download/dotnet/9.0

**Steps:**
git clone https://github.com/sheikharbazalam/Unit-Conversion-API.git
cd Unit-Conversion-API
dotnet restore
dotnet run --project src/UnitConverter.API

open you browser at http://localhost:5000 - SWAGGER UI load automatically

**Run Tests**
dotnet test
All 31 tests should pass.

**API Endpoints**
Conversion


POST
/api/v1/Conversion/convert - Convert a Single Value


POST
/api/v1/Conversion/convert/batch - Convert upto 20 value at once

Health


GET
/api/v1/Health - Health Check

History


GET
/api/v1/History - View Recent Conversions

Units


GET
/api/v1/Units - List of Supported Units


GET
/api/v1/Units/categories - List of All Categories

GET
/api/v1/units?category.  - TemperatureFilter units by category


Example of Results:
POST
/api/v1/Conversion/convert
{
  "value": 100,
  "from": "celsius",
  "to": "fahrenheit"
}

Output:
{
  "inputValue": 100,
  "fromUnit": "Celsius",
  "fromSymbol": "C",
  "outputValue": 212,
  "toUnit": "Fahrenheit",
  "toSymbol": "F",
  "category": "Temperature",
  "formula": "(x * 1) + 273.15 -> (result - 255.3722222222222) / 0.5555555555555556"
}

**Design Decisions and Trade-offs**
Clean Architecture: Domain, Application, Infrastructure, and API are the four levels that make up the solution. Because business logic is fully contained in the Application layer and does not rely on ASP.NET Core, it can be tested independently.  Each unit in the linear conversion model holds a Factor and Offset in relation to a SI base unit. SI = (input × Factor) + Offset is the formula. This implies that introducing a new unit simply necessitates one line in the seed file; business logic is never altered.  In order to prevent floating-point accumulation issues that could result in readings like 212.000004 instead of 212, Fahrenheit utilises Offset=2298.35/9.0 instead of a hardcoded decimal.  In-memory storage: For ease of use, unit definitions and history are kept in memory. Because of the IUnitRepository and IConversionHistoryRepository interfaces, these can be swapped to CosmosDB or PostgreSQL with zero changes to business logic.

Alias System: Units can be looked up by key or any alias, all case-insensitive. The repository builds a dictionary at startup for O(1) lookups.

RFC 7807 Problem Details  - All errors return a consistent machine-readable format, making it easy for API consumers to handle errors programmatically.