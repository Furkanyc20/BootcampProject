using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BootcampProject.Core.Exceptions;

namespace BootcampProject.Core.Middleware
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var errorResponse = exception switch
            {
                BusinessException businessEx => new ErrorResponse(
                    businessEx.Message,
                    (int)HttpStatusCode.BadRequest,
                    "Business rule violation"
                ),
                
                NotFoundException notFoundEx => new ErrorResponse(
                    notFoundEx.Message,
                    (int)HttpStatusCode.NotFound,
                    "Resource not found"
                ),
                
                ValidationException validationEx => new ErrorResponse(
                    validationEx.Message,
                    (int)HttpStatusCode.BadRequest,
                    CreateValidationDetails(validationEx)
                ),
                
                DuplicateException duplicateEx => new ErrorResponse(
                    duplicateEx.Message,
                    (int)HttpStatusCode.Conflict,
                    "Duplicate resource"
                ),
                
                UnauthorizedAccessException unauthorizedEx => new ErrorResponse(
                    unauthorizedEx.Message,
                    (int)HttpStatusCode.Unauthorized,
                    "Authentication required"
                ),
                
                ArgumentException argumentEx => new ErrorResponse(
                    argumentEx.Message,
                    (int)HttpStatusCode.BadRequest,
                    "Invalid argument provided"
                ),
                
                InvalidOperationException invalidOpEx => new ErrorResponse(
                    invalidOpEx.Message,
                    (int)HttpStatusCode.BadRequest,
                    "Invalid operation"
                ),
                
                _ => new ErrorResponse(
                    "An unexpected error occurred. Please try again later.",
                    (int)HttpStatusCode.InternalServerError,
                    "Internal server error"
                )
            };

            // Log different exception types with appropriate log levels
            switch (exception)
            {
                case BusinessException:
                case ValidationException:
                case DuplicateException:
                case NotFoundException:
                case ArgumentException:
                case InvalidOperationException:
                    _logger.LogWarning(exception, "Client error occurred: {Message}", exception.Message);
                    break;
                
                case UnauthorizedAccessException:
                    _logger.LogWarning(exception, "Unauthorized access attempt: {Message}", exception.Message);
                    break;
                
                default:
                    _logger.LogError(exception, "Internal server error occurred: {Message}", exception.Message);
                    break;
            }

            context.Response.StatusCode = errorResponse.StatusCode;

            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private string CreateValidationDetails(ValidationException validationException)
        {
            if (validationException.Errors == null || validationException.Errors.Count == 0)
            {
                return "Validation failed";
            }

            var validationDetails = new
            {
                errors = validationException.Errors
            };

            return JsonSerializer.Serialize(validationDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}