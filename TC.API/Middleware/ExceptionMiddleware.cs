using TC.Application.Exceptions;

namespace TC.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AccessDeniedException ex)
            {
                _logger.LogWarning(ex, "Access denied exception.");
                await HandleAccessDeniedExceptionAsync(context, ex);
            }
            catch(AlreadyExistException ex)
            {
                _logger.LogWarning(ex, "Already exist exception.");
                await HandleAlreadyExistExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError);
            }
        }

        private static Task HandleAccessDeniedExceptionAsync(HttpContext context, AccessDeniedException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            var response = new
            {
                StatusCode = StatusCodes.Status403Forbidden,
                ErrorCode = "ACCESS_DENIED",
                Message = exception.Message,
                Details = exception.InnerException?.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }

        private static Task HandleAlreadyExistExceptionAsync(HttpContext context, AlreadyExistException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            var response = new
            {
                StatusCode = StatusCodes.Status403Forbidden,
                ErrorCode = "ALREADY_EXIST",
                Message = exception.Message,
                Details = exception.InnerException?.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = "An unexpected error occurred.",
                Details = exception.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }

}
