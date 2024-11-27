using Microsoft.AspNetCore.Diagnostics;

namespace SurvayBasket.ErrorHandling
{
    public class ExceptionHandlerMiddlWare : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandlerMiddlWare> logger;

        public ExceptionHandlerMiddlWare(ILogger<ExceptionHandlerMiddlWare> logger)
        {
            this.logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            logger.LogError(exception, exception.Message);
            var Issue = new
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",


            };
            await httpContext.Response.WriteAsJsonAsync(Issue, cancellationToken);
            return true;
        }
    }
}
