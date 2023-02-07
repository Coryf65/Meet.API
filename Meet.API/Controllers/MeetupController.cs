using AutoMapper;
using Meet.API.Authorization;
using Meet.API.Entities;
using Meet.API.Filters;
using Meet.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Meet.API.Data;

[ApiController]
[Route("api/meetup")]
[Authorize] // all actions require a valid login / jwt
[ServiceFilter(typeof(TimeTrackFilter))]
public class MeetupController : Controller
{
	private readonly MeetupContext _context;
	private readonly IMapper _mapper;
	private readonly IAuthorizationService _authorizationService;

	public MeetupController(MeetupContext context, IMapper mapper, IAuthorizationService authorizationService)
	{
		_context = context;
		_mapper = mapper;
		_authorizationService = authorizationService;
	}

	/// <summary>
	/// Get all Meetups Or Get meetups by name
	/// </summary>
	/// <returns>All Meetups</returns>
	[HttpGet]
	[AllowAnonymous] // allows not logged in requests
	public ActionResult<List<MeetupDetailsDTO>> GetAll([FromQuery] string? searchPhrase)
	{
		List<Meetup> meetups = _context.Meetups
			.Include(m => m.Location)
			.Where(m => searchPhrase == null || (m.Organizer.ToLower().Contains(searchPhrase.ToLower()) || m.Name.Contains(searchPhrase.ToLower())))
			.ToList();
		List<MeetupDetailsDTO> meetupsDtos = _mapper.Map<List<MeetupDetailsDTO>>(meetups);

		return Ok(meetupsDtos);
	}

	/// <summary>
	/// Get a Meetup by the Name
	/// </summary>
	/// <param name="name">name of the meetup</param>
	/// <returns></returns>
	[HttpGet("{name}")]
	[Authorize(Policy = "18AndOlder")] // custom Policy to allow only if conditions are met
	public ActionResult<MeetupDetailsDTO> Get(string name)
	{
		var meetup = _context.Meetups
			.Include(m => m.Location)
			.Include(m => m.Lectures)
			// replacing spaces with dashes
			.FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

		// the name is not found
		if (meetup is null)
			return NotFound($"A Meetup with the name: '{name}' is not found.");

		// DTO conversion
		var meetupDto = _mapper.Map<MeetupDetailsDTO>(meetup);

		// name found
		return Ok(meetupDto);
	}

	/// <summary>
	/// Save a new Meetup
	/// </summary>
	/// <param name="model">Json data for the meetup</param>
	/// <returns></returns>
	[HttpPost]
	[Authorize(Roles = "Admin,Moderator")]
	public ActionResult Post([FromBody] MeetupDTO model)
	{
		// checking for a valid request
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var meetup = _mapper.Map<Meetup>(model);

		var userId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

		meetup.CreatedById = int.Parse(userId);

		_context.Meetups.Add(meetup);
		_context.SaveChanges();

		var key = meetup.Name.Replace(" ", "-").ToLower();

		return Created($"api/meetup/{key}", null);
	}

	/// <summary>
	/// A Update to change existing Meetups
	/// </summary>
	/// <param name="name"></param>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPut("{name}")]
	public ActionResult Put(string name, [FromBody] MeetupDTO model)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var meetup = _context.Meetups
			// replacing spaces with dashes
			.FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

		// the name is not found
		if (meetup is null)
			return NotFound($"A Meetup with the name: '{name}' is not found.");

		var authorizationResult = _authorizationService.AuthorizeAsync(
			user: User,
			resource: meetup,
			requirement: new ResourceOperationRequirement(OperationType.Update)
		).Result;

		if (!authorizationResult.Succeeded)
			return Forbid("User Not Authorized to Access.");

		meetup.Name = model.Name;
		meetup.Organizer = model.Organizer;
		meetup.Date = model.Date;
		meetup.IsPrivate = model.IsPrivate;

		_context.SaveChanges();

		return NoContent();
	}

	/// <summary>
	/// Delete a Meetup
	/// </summary>
	/// <param name="name">Name of the meetup</param>
	/// <returns>NotFound / NoContent</returns>
	[HttpDelete("{name}")]
	public ActionResult Delete(string name)
	{
		var meetup = _context.Meetups
			// replacing spaces with dashes
			.FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

		// the name is not found
		if (meetup is null)
			return NotFound($"A Meetup with the name: '{name}' is not found.");

		var authorizationResult = _authorizationService.AuthorizeAsync(
			user: User,
			resource: meetup,
			requirement: new ResourceOperationRequirement(OperationType.Delete)
		).Result;

		if (!authorizationResult.Succeeded)
			return Forbid("User Not Authorized to Access.");

		_context.Remove(meetup);
		_context.SaveChanges();

		return NoContent();
	}
}