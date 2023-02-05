using System.ComponentModel.DataAnnotations;

namespace Meet.API.Models;

public class RegisterUserDTO
{
	// removing these validation to use fluentvalidation nuget instead [Required]
	public string Email { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
	public DateTime? DateOfBirth { get; set; } = DateTime.MinValue;
	public int RoleId { get; set; } = 1; // set to default as a "User"
}
