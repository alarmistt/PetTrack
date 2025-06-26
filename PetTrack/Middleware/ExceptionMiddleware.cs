using PetTrack.Core.Constants;
using PetTrack.Core.Enums;
using PetTrack.Core.Exceptions;
using PetTrack.Core.Models;
using System.Text.Json;

namespace PetTrack.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ErrorException ex)
            {
                _logger.LogWarning(ex, $"Handled business exception at path {context.Request.Path}");

                if (!context.Response.HasStarted)
                {
                    await HandleExceptionAsync(context, ex.StatusCode, ex.ErrorDetail);
                }
                else
                {
                    _logger.LogWarning("❌ Response already started — cannot handle ErrorException");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled exception at path {context.Request.Path}");

                if (!context.Response.HasStarted)
                {
                    var error = new ErrorDetail
                    {
                        ErrorMessage = "An unexpected error occurred.",
                        ErrorCode = ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                    };

                    await HandleExceptionAsync(context, (int)StatusCodeHelper.ServerError, error);
                }
                else
                {
                    _logger.LogWarning("❌ Response already started — cannot handle unhandled exception");
                }
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, int statusCode, ErrorDetail errorDetail)
        {
            if (context.Response.HasStarted)
            {
                Console.WriteLine("❌ Cannot write to response, already started.");
                return;
            }

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new
            {
                status = statusCode,
                errorCode = errorDetail.ErrorCode,
                message = errorDetail.ErrorMessage
            });

            await context.Response.WriteAsync(result);
        }
    }


    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
