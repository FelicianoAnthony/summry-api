using Newtonsoft.Json;
using StarterApi.ApiModels.Middlewares;
using System.Net;
using System.Security.Authentication;

namespace StarterApi.Middlewares.Exceptions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                Exception innerMostException = GetInnerMostException(error);

                string serializedResponse;
                int statusCode;
                switch (innerMostException)
                {
                    case AuthenticationException:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        serializedResponse = CreateSerializedExceptionResponse(innerMostException, statusCode);
                        response.StatusCode = statusCode;
                        _logger.LogError(error, error?.Message);
                        break;
                    case NotFoundException:
                        statusCode = (int)HttpStatusCode.NotFound;
                        serializedResponse = CreateSerializedExceptionResponse(innerMostException, statusCode);
                        response.StatusCode = statusCode;
                        _logger.LogError(error, error?.Message);
                        break;
                    case BadHttpRequestException:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        serializedResponse = CreateSerializedExceptionResponse(innerMostException, statusCode);
                        response.StatusCode = statusCode;
                        _logger.LogError(error, error?.Message);
                        break;
                    default:
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        serializedResponse = CreateSerializedExceptionResponse(innerMostException, statusCode);
                        response.StatusCode = statusCode;
                        _logger.LogCritical(error, error?.Message);
                        break;
                }

                await response.WriteAsync(serializedResponse);
            }
        }

        private string CreateSerializedExceptionResponse(Exception error, int statusCode)
        {
            ExceptionResponse exceptionResponse = new()
            {
                Type = error.GetType().Name,
                Title = error.Message,
                Status = statusCode,
                TraceId = DateTime.UtcNow.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                Errors = GenerateErrorProperty(error)
            };

            return SerializeResponse(exceptionResponse);
        }


        private Dictionary<string, List<string>> GenerateErrorProperty(Exception exception)
        {
            return new Dictionary<string, List<string>>() { { exception.GetType().Name, new List<string>() { { exception.Message } } } };
        }

        private Exception GetInnerMostException(Exception exception)
        {
            if (exception.InnerException != null)
            { 
                return GetInnerMostException(exception.InnerException);
            }
            return exception;
        }


        private string SerializeResponse(ExceptionResponse errorResponse)
        {
            return JsonConvert.SerializeObject(errorResponse);
        }

    }

}
