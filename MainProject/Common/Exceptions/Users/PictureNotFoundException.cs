using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.Users
{
    public class PictureNotFoundException : CustomException
    {
        public PictureNotFoundException(string userId)
            : base($"User with id {userId} does not have a picture.")
        {

        }

    }
}
