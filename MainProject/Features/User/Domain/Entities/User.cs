using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Features.User.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(20)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(20)]
    public string LastName { get; set; }

    public bool IsSuspended { get; set; } = false;

    public string? ProfileImagePath { get; set; }

	public bool IsDeleted { get; set; } = false;

}


