using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(
               string message,
               HttpStatusCode statusCode = HttpStatusCode.Unauthorized
            ): base(message)
        {


            StatusCode = statusCode;
        }
    }
}

