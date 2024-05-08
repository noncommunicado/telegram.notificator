
namespace WebApi.Middlewares;

public sealed record ExceptionResponse(int StatusCode, string Message = "", IEnumerable<ExceptionResponseError>? Errors = null);

public sealed record ExceptionResponseError(string PropertyName, string ErrorMessage);