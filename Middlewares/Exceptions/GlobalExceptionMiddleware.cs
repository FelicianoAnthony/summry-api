using Newtonsoft.Json;
using StarterApi.ApiModels.Middlewares;
using System.Diagnostics;
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

                string serializedResponse;
                switch (error)
                {
                    case AuthenticationException:
                        serializedResponse = CreateSerializedExceptionResponse(error, true);
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        _logger.LogError(error, error?.Message);
                        break;
                    case NotFoundException:
                        serializedResponse = CreateSerializedExceptionResponse(error, true);
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        _logger.LogError(error, error?.Message);
                        break;
                    case BadHttpRequestException:
                        serializedResponse = CreateSerializedExceptionResponse(error, true);
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        _logger.LogError(error, error?.Message);
                        break;
                    default:
                        serializedResponse = CreateSerializedExceptionResponse(error, false);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        _logger.LogCritical(error, error?.Message);
                        break;
                }

                await response.WriteAsync(serializedResponse);
            }
        }

        private string CreateSerializedExceptionResponse(Exception error, bool isExceptionHandled)
        {
            ExceptionResponse exceptionResponse = new()
            {
                ExceptionType = error.GetType().Name,
                Error = error?.Message,
                InnerException = error.InnerException?.ToString().Split("\r\n").Select(e => e.TrimStart()).ToList(),
                ExceptionHandled = isExceptionHandled,
                Timestamp = DateTime.UtcNow,
            };

            ExceptionCodeDetails codeDetails = ParseStackTrace(error);
            exceptionResponse.Class = codeDetails.Class;
            exceptionResponse.Method = codeDetails.Method;

            return SerializeResponse(exceptionResponse);
        }


        private string SerializeResponse(ExceptionResponse errorResponse)
        {
            return JsonConvert.SerializeObject(errorResponse);
        }


        private ExceptionCodeDetails ParseStackTrace(Exception error)
        {
            StackTrace st = new(error, true);
            var query = st.GetFrames()
                        .Select(frame => new ExceptionCodeDetails
                        {
                            FileName = frame.GetFileName(),
                            LineNumber = frame.GetFileLineNumber(),
                            ColumnNumber = frame.GetFileColumnNumber(),
                            Method = frame.GetMethod()?.ToString(),
                            Class = frame.GetMethod().DeclaringType?.ToString()
                        }).FirstOrDefault();

            return query;
        }
    }

}
