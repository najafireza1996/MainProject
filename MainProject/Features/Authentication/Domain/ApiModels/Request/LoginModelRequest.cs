using System.ComponentModel.DataAnnotations;

namespace Features.Authentication.Domain.ApiModels.Request
{
    public class LoginModelRequest
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
