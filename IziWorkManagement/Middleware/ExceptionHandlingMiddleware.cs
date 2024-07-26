using IziWork.Business.DTO;
using IziWork.Common.DTO;
using IziWorkManagement.Middleware.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using KeyNotFoundException = IziWorkManagement.Middleware.Exceptions.KeyNotFoundException;
using NotImplementedException = IziWorkManagement.Middleware.Exceptions.NotImplementedException;
using UnauthorizedAccessException = IziWorkManagement.Middleware.Exceptions.UnauthorizedAccessException;

namespace IziWorkManagement.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ResultDTO { };
            var exceptionType = exception.GetType();
            if (exceptionType == typeof(BadRequestException))
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Messages = new List<string> { "Đã xảy ra lỗi" };
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Messages = new List<string> { "Đã xảy ra lỗi" };
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                response.StatusCode = (int)HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Messages = new List<string> { "Đã xảy ra lỗi" };
            }
            else if (exceptionType == typeof(KeyNotFoundException))
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Messages = new List<string> { "Đã xảy ra lỗi" };
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Messages = new List<string> { string.Format("Đã xảy ra lỗi - [Chi tiết: {0}]", exception.Message + " | - | " + exception.InnerException?.Message) };
            }

            _logger.LogError("Đã xảy ra lỗi - {exception}", exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
