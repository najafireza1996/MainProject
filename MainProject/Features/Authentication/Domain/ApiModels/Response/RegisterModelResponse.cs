using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.Authentication.Domain.ApiModels.Response
{
    public class RegisterModelResponse
    {
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
