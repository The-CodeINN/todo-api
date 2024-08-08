using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Exceptions.HandlerMiddleware
{
    public class CustomExceptionHandlerMiddleware : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("Error Message: {ExceptionMessage}, Time of occurrence {Time}", exception.Message, DateTime.UtcNow);

            var details = exception switch
            {
                InternalServerException => (Message: exception.Message, Name: exception.GetType().Name, StatusCode: StatusCodes.Status500InternalServerError),
                ValidationException => (Message: exception.Message, Name: exception.GetType().Name, StatusCode: StatusCodes.Status400BadRequest),
                BadRequestException => (Message: exception.Message, Name: exception.GetType().Name, StatusCode: StatusCodes.Status400BadRequest),
                NotFoundException => (Message: exception.Message, Name: exception.GetType().Name, StatusCode: StatusCodes.Status404NotFound),
                _ => (Message: exception.Message, Name: exception.GetType().Name, StatusCode: StatusCodes.Status500InternalServerError)
            };

            httpContext.Response.StatusCode = details.StatusCode;

            var problemDetails = new ProblemDetails
            {
                Title = details.Name,
                Status = details.StatusCode,
                Detail = details.Message
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
            return true;
        }
    }
}
