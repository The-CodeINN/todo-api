using FluentValidation;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TodoApi.DTOs;

namespace TodoApi.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                IValidator validator = GetValidator(context);

                if (validator != null)
                {
                    var body = await GetRequestBodyAsync(context);

                    if (!string.IsNullOrEmpty(body))
                    {
                        FluentValidation.Results.ValidationResult validationResult = null;
                        try
                        {
                            validationResult = await ValidateBodyAsync(validator, body);
                        }
                        catch (JsonReaderException)
                        {
                            await RespondWithInvalidJsonErrorAsync(context);
                            return;
                        }

                        if (validationResult != null && !validationResult.IsValid)
                        {
                            await RespondWithValidationErrorsAsync(context, validationResult);
                            return;
                        }
                    }
                    else
                    {
                        await RespondWithInvalidJsonErrorAsync(context);
                        return;
                    }
                }
            }

            await _next(context);
        }

        private static IValidator GetValidator(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post)
            {
                return context.RequestServices.GetService<IValidator<TodoItemCreateDto>>();
            }
            else if (context.Request.Method == HttpMethods.Put)
            {
                return context.RequestServices.GetService<IValidator<TodoItemDto>>();
            }

            return null;
        }

        private static async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;
            return body;
        }

        private static async Task<FluentValidation.Results.ValidationResult> ValidateBodyAsync(IValidator validator, string body)
        {
            object dto = null;
            if (validator is IValidator<TodoItemCreateDto>)
            {
                dto = JsonConvert.DeserializeObject<TodoItemCreateDto>(body);
            }
            else if (validator is IValidator<TodoItemDto>)
            {
                dto = JsonConvert.DeserializeObject<TodoItemDto>(body);
            }

            if (dto != null)
            {
                return await validator.ValidateAsync(new ValidationContext<object>(dto));
            }

            return null;
        }

        private static async Task RespondWithValidationErrorsAsync(HttpContext context, FluentValidation.Results.ValidationResult validationResult)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var response = new
            {
                Errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private static async Task RespondWithInvalidJsonErrorAsync(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var response = new { Error = "Invalid JSON format" };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
