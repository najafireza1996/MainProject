using System.Net;

namespace Common.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}