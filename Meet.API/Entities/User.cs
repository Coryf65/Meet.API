namespace Meet.API.Entities;

public class User
{
	public int Id { get; set; }
	public string Email { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public DateTime? DateOfBirth { get; set; }
	public string PasswordHash { get; set; } = string.Empty;

	public int RoleId { get; set; }
	public Role Role { get; set; }
}