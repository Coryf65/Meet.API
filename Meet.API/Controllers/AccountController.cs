using Meet.API.Data;
using Meet.API.Entities;
using Meet.API.Identity;
using Meet.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly MeetupContext _context;
	private readonly IPasswordHasher<User> _passwordHaser;
	private readonly IJwtProvider _jwtProvider;

	public AccountController(MeetupContext meetupContext, IPasswordHasher<User> passwordHasher, IJwtProvider jwtProvider)
	{
		_context = meetupContext;
		_passwordHaser = passwordHasher;
		_jwtProvider = jwtProvider;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="registerUserDTO"></param>
	/// <returns></returns>
	[HttpPost("register")]
	public ActionResult Register([FromBody] RegisterUserDTO registerUserDTO)
	{
		// checking for a valid request
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		// checking the user mode for data validation


		User newUser = new()
		{
			Email = registerUserDTO.Email,
			DateOfBirth = registerUserDTO.DateOfBirth,
			RoleId = registerUserDTO.RoleId
		};

		string passwordHash = _passwordHaser.HashPassword(newUser, registerUserDTO.Password);
		newUser.PasswordHash = passwordHash;

		_context.Users.Add(newUser);
		_context.SaveChanges();

		return Ok();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="userLoginDTO"></param>
	/// <returns></returns>
	[HttpPost("login")]
	public ActionResult Login([FromBody] UserLoginDTO userLoginDTO)
	{
		var user = _context.Users
			.Include(user => user.Role)
			.FirstOrDefault(user => user.Email == userLoginDTO.Email);

		if (user is null)
			return BadRequest("Invalid username or password.");

		var userVerify = _passwordHaser.VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password);

		if (userVerify == PasswordVerificationResult.Failed)
			return BadRequest("Invalid username or password.");

		// generate jwt
		var token = _jwtProvider.GenerateJwt(user);

		// send token back to user
		return Ok(token);
	}
}
