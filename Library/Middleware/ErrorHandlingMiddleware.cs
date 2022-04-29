using Library.Domain.Entities;
using Library.Infrastructure.Crosscutting.Abstract;
using System.Net;

namespace Library.Application.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, ILogService logService)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                try
                {
                    await logService.LogAsync(new Error
                    {
                        Message = exception.ToString(),
                        LoggedAt = DateTime.UtcNow
                    });
                }
                catch
                {
                    // We could log to a file here if something goes wrong while saving on DB
                }

                await HandleExceptionAsync(httpContext);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(context.Response.StatusCode.ToString());
        }
    }
}
