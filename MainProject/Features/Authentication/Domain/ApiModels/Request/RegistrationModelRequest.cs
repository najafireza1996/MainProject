using System.ComponentModel.DataAnnotations;

namespace Features.Authentication.Domain.ApiModels.Request
{
    
    public class RegistrationModelRequest
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string FirstName { get; set; }
        [MinLength(3)]
        [MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string LastName { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        [RegularExpression(@"^[a-z0-9]+$")]
        [Display(Name = "Username")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 4)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(50, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
