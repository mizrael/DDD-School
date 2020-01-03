using DDD.School.Commands;
using System;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DDD.School.API.Middlewares
{

    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionsMiddleware> _logger;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger, ProblemDetailsFactory problemDetailsFactory)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(logger));
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
            var status = ExtractHttpStatus(ex);

            var problemDetails = BuildProblemDetails(ex, status, context);
            var detailsType = problemDetails.GetType();
            var jsonData = System.Text.Json.JsonSerializer.Serialize(problemDetails, detailsType);

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/problem+json";
            
            await context.Response.WriteAsync(jsonData);

            _logger.LogError(ex, ex.Message);
        }

        private ProblemDetails BuildProblemDetails<TEx>(TEx ex, int status, HttpContext context) where TEx : Exception
        {
            if (ex is ValidationException valEx)
            {
                var state = new ModelStateDictionary();
                foreach (var err in valEx.Errors)
                    state.AddModelError(err.Context, err.Message);
                return _problemDetailsFactory.CreateValidationProblemDetails(context, state, status, ex.Message, null, null, context.Request.Path);
            }

            return _problemDetailsFactory.CreateProblemDetails(context, status, ex.Message, null, null, context.Request.Path);      
        }

        private static int ExtractHttpStatus<TEx>(TEx ex) where TEx : Exception
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

            return (int)status;
        }
    }
}
