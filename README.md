A RESTful ASP.NET Core 9 Web API converting numerical values between units of measurement.

## Features

- 6 conversion categories : Length, Temperature, Weight, Volume, speed and Area.
- 40+ units with alias support (e.g. `m`, `metre`, `meter` all work)
- Batch conversion endpoint (up to 20 at once)
- Conversion history endpoint
- Unit catalogue endpoint for discovery
- RFC 7807 Problem Details error responses
- Swagger UI at `/`
- Rate limiting (100 requests/minute)
- Structured logging via Serilog
- Clean Architecture (Domain / Application / Infrastructure / API)
- Unit tests with xUnit, FluentAssertions, NSubstitute

## Project Structure