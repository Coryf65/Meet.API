using Meet.API.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Meet.API.Identity;

public class JwtProvider : IJwtProvider
{
	private readonly JwtOptions _jwtOptions;

	public JwtProvider(JwtOptions jwtOptions)
	{
		_jwtOptions = jwtOptions;
	}

	/// <summary>
	/// Create a Json Web Token for our User
	/// </summary>
	/// <param name="user">User data</param>
	/// <returns>a JWT</returns>
	public string GenerateJwt(User user)
	{
		List<Claim> claims = new()
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Role, user.Role.RoleName),
			new Claim(ClaimTypes.Name, user.Email),
			new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("g"))
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
		var expires = DateTime.Now.AddDays(_jwtOptions.JwtExpireDays);

		var token = new JwtSecurityToken(
		
			issuer: _jwtOptions.JwtIssuer,
			audience: _jwtOptions.JwtIssuer,
			claims: claims,
			expires: expires,
			signingCredentials: creds		
		);

		var tokenHandler = new JwtSecurityTokenHandler();

		return tokenHandler.WriteToken(token);
	}
}
