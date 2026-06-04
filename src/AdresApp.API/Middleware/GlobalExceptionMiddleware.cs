using System.Net;
using System.Text.Json;
using AdresApp.Application.DTOs;
using AdresApp.Domain.Exceptions;
using FluentValidation;

namespace AdresApp.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepcion no controlada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, mensaje) = exception switch
        {
            ValidationException ve => (HttpStatusCode.BadRequest, string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),
            NotFoundException nfe  => (HttpStatusCode.NotFound, nfe.Message),
            ExternalServiceException ese => (HttpStatusCode.BadGateway, ese.Message),
            DomainException de     => (HttpStatusCode.BadRequest, de.Message),
            ArgumentException ae   => (HttpStatusCode.BadRequest, ae.Message),
            _                      => (HttpStatusCode.InternalServerError, "Error interno. Intente mas tarde.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        var json = JsonSerializer.Serialize(
            ApiResponseDto<object>.Error(mensaje),
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}
