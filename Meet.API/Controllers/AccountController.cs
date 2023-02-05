using Meet.API.Data;
using Meet.API.Entities;
using Meet.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meet.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
	private readonly MeetupContext _context;

	public AccountController(MeetupContext meetupContext)
	{
		_context = meetupContext;
	}

	[HttpPost("register")]
	public ActionResult Register([FromBody] RegisterUserDTO registerUserDTO)
	{
		// checking for a valid request
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		User newUser = new()
		{
			Email = registerUserDTO.Email,
			DateOfBirth = registerUserDTO.DateOfBirth,
			RoleId = registerUserDTO.RoleId
		};

		_context.Users.Add(newUser);
		_context.SaveChanges();

		return Ok();
	}
}
