using Meet.API.Entities;

namespace Meet.API.Identity;

public interface IJwtProvider
{
	string GenerateJwt(User user);
}
