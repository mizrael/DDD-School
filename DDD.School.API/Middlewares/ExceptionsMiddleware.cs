using DDD.School.Commands;
using System;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDD.School.API.Middlewares
{

    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionsMiddleware> _logger;

        public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {            
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            var error = ExtractError(ex);
            string jsonData = System.Text.Json.JsonSerializer.Serialize(error);

            context.Response.StatusCode = (int)ExtractHttpStatus(ex);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(jsonData);

            _logger.LogError(ex, ex.Message);
        }

        private static dynamic ExtractError<TEx>(TEx ex) where TEx : Exception
        {
            if (ex is ValidationException valEx)
            {
                return new
                {
                    message = valEx.Message,
                    data = valEx.Errors
                };
            }

            return new
            {
                message = ex.Message
            };
        }

        private static HttpStatusCode ExtractHttpStatus<TEx>(TEx ex) where TEx : Exception
        {
            var status = HttpStatusCode.InternalServerError;

            if (ex is ValidationException ||
                ex is ArgumentNullException ||
                ex is ArgumentOutOfRangeException ||
                ex is ArgumentException ||
                ex is HttpRequestException)
                status = HttpStatusCode.BadRequest;
            else if (ex is AuthenticationException)
                status = HttpStatusCode.Unauthorized;
            else if (ex is UnauthorizedAccessException)
                status = HttpStatusCode.Forbidden;

            return status;
        }
    }
}
