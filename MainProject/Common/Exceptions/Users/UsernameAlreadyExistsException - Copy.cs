﻿using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.Users
{
    public class NotValidUsernameException : CustomException
    {
        public NotValidUsernameException(string username)
            : base($"Username {username} is not valid!")
        {

        }
    }
}
