using System.Net;

namespace Common.Exceptions;

public class ApiException : CustomException
{
    public ApiException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError
        ) : base(message)
    {
        StatusCode = statusCode;
    }
}
