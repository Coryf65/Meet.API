using AutoMapper;
using Meet.API.Entities;
using Meet.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.API.Data;

[Route("api/meetup")]
public class MeetupController : Controller
{
	private readonly MeetupContext _context;
	private readonly IMapper _mapper;

	public MeetupController(MeetupContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	/// <summary>
	/// Get all Meetups
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public ActionResult<List<MeetupDetailsDTO>> Get()
	{
		List<Meetup> meetups = _context.Meetups.Include(m => m.Location).ToList();
		List<MeetupDetailsDTO> meetupsDtos = _mapper.Map<List<MeetupDetailsDTO>>(meetups);

		return Ok(meetupsDtos);
	}

	[HttpGet("{name}")]
	public ActionResult<MeetupDetailsDTO> Get(string name)
	{
		var meetup = _context.Meetups
			.Include(m => m.Location)
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

	[HttpPost]
	public ActionResult Post([FromBody] MeetupDTO model)
	{
		// checking for a valid request
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var meetup = _mapper.Map<Meetup>(model);

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

		_context.Remove(meetup);
		_context.SaveChanges();

		return NoContent();
	}
}