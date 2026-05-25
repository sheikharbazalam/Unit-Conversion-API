using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UnitConverter.Domain.Entities;

namespace UnitConverter.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Errors}", string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)));
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, "Validation Error",
                string.Join(" | ", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (CategoryMismatchException ex)
        {
            _logger.LogWarning("Category mismatch: {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new CategoryMismatchResponse(
                Status: (int)HttpStatusCode.UnprocessableEntity,
                Title: "Category Mismatch",
                Detail: ex.Message,
                Suggestions: new SuggestionDetails(
                    FromUnit: ex.FromUnit,
                    Category: ex.FromCategory,
                    CanConvertTo: ex.CanConvertTo
                )
            ));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogWarning("Out of range: {Message}", ex.Message);
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, "Out of Range", ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Bad argument: {Message}", ex.Message);
            await WriteProblemAsync(context, HttpStatusCode.UnprocessableEntity, "Invalid Input", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError,
                "Internal Server Error", "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemAsync(
        HttpContext ctx, HttpStatusCode status, string title, string detail)
    {
        ctx.Response.StatusCode = (int)status;
        ctx.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            Detail = detail,
            Instance = ctx.Request.Path
        };

        await ctx.Response.WriteAsJsonAsync(problem);
    }
}

public sealed record SuggestionDetails(string FromUnit, string Category, IReadOnlyList<string> CanConvertTo);
public sealed record CategoryMismatchResponse(int Status, string Title, string Detail, SuggestionDetails Suggestions);