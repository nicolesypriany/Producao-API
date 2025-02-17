using System.Net;
using System.Text.Json;

namespace ProducaoAPI
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (HttpStatusCodeException ex)
            {
                await HandleHttpStatusCodeExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleHttpStatusCodeExceptionAsync(HttpContext context, HttpStatusCodeException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;

            var response = new
            {
                context.Response.StatusCode,
                Message = exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }

        private static Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                context.Response.StatusCode,
                Message = "Ocorreu um erro interno no servidor."
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}