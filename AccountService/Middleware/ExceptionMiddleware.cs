using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace AccountService.Middleware
{


    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode status;
            string message = ex.Message;

            switch (ex)
            {
                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    break;

                case UnauthorizedAccessException:
                    status = HttpStatusCode.Unauthorized;
                    break;

                case ArgumentException:
                    status = HttpStatusCode.BadRequest;
                    break;
                case DbUpdateException:
                    status = HttpStatusCode.Conflict;
                    break;
                case AlreadyExistsException:
                    status = HttpStatusCode.Conflict;
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var response = JsonSerializer.Serialize(new
            {
                error = message,
                status = (int)status
            });

            return context.Response.WriteAsync(response);
        }
    }
}
