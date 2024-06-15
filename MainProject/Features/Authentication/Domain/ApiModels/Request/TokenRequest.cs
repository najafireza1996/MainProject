using System.ComponentModel.DataAnnotations;

namespace Features.Authentication.Domain.ApiModels.Request
{
    public class TokenRequest
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }

}
