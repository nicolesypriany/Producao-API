using System.Net;
using System.Text.Json;

namespace ProducaoAPI.Exceptions
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
            catch(NotFoundException ex)
            {
                await HandleNotFoundExceptionAsync(httpContext, ex);
            }
            catch(BadRequestException ex)
            {
                await HandleBadRequestExceptionAsync(httpContext, ex);
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
                exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }

        private static Task HandleBadRequestExceptionAsync(HttpContext context, BadRequestException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;

            var response = new
            {
                context.Response.StatusCode,
                exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }

        private static Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;

            var response = new
            {
                context.Response.StatusCode,
                exception.Message
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
                //Message = "Ocorreu um erro interno no servidor."
                Message = exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}