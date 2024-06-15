using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.User.Domain.ApiModels.RequestDTO
{
    public class ChangeUsernameRequest
    {
        public string CurrentUsername { get; set; }
        public string NewUsername { get; set; }
    }
}
