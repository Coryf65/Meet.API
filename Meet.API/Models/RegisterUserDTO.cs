using System.ComponentModel.DataAnnotations;

namespace Meet.API.Models;

public class RegisterUserDTO
{
	[Required]
	public string Email { get; set; }
	[Required]
	[MinLength(3)] // for testing
	public string Password { get; set; }
	public DateTime? DateOfBirth { get; set; } = DateTime.MinValue;
	public int RoleId { get; set; } = 1; // set to default as a "User"
}
