namespace Meet.API.Identity;

public class JwtOptions
{
	public string JwtKey { get; set; }
	public string JwtIssuer { get; set; }
	public int JwtExpireDays { get; set; }
}
