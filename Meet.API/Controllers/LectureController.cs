using AutoMapper;
using Meet.API.Data;
using Meet.API.Entities;
using Meet.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.API.Controllers;

[ApiController]
[Route("api/meetup/{meetupName}/lecture")]
public class LectureController : Controller
{
	private readonly MeetupContext _context;
	private readonly IMapper _mapper;
	private readonly ILogger<LectureController> _logger;

	public LectureController(MeetupContext context, IMapper mapper, ILogger<LectureController> logger)
	{
		_context = context;
		_mapper = mapper;
		_logger = logger;
	}

	/// <summary>
	/// Get a Lecture by name
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	[HttpGet]
	public ActionResult Get(string meetupName)
	{
		var meetup = _context.Meetups
			.Include(m => m.Lectures)
			.FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

		if (meetup is null)
			return NotFound($"A Meetup with the name: '{meetup}' is not found.");

		var lectures = _mapper.Map<List<LectureDTO>>(meetup.Lectures);

		return Ok(lectures);
	}

	/// <summary>
	/// Save a new lecture
	/// </summary>
	/// <param name="meetupName"></param>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPost]
	public ActionResult Post(string meetupName, [FromBody] LectureDTO model)
	{
		// checking for a valid request
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var meetup = _context.Meetups
			.Include(m => m.Lectures)
			.FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

		// the name is not found
		if (meetup is null)
			return NotFound($"A Meetup with the name: '{meetup}' is not found.");

		var lecture = _mapper.Map<Lecture>(model);

		meetup.Lectures.Add(lecture);
		_context.SaveChanges();

		return Created($"api/meetup/{meetupName}", null);
	}

	/// <summary>
	/// Delete ALL lectures for a Meetup
	/// </summary>
	/// <param name="meetupName"></param>
	/// <returns></returns>
	[HttpDelete]
	public ActionResult Delete(string meetupName)
	{
		var meetup = _context.Meetups
			.Include(m => m.Lectures)
			.FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

		if (meetup is null)
			return NotFound($"A Meetup with the name: '{meetup}' is not found.");

		_logger.LogWarning("All lectures for meetup '{meetupName}' have been deleted.", meetupName);

		_context.Lectures.RemoveRange(meetup.Lectures);
		_context.SaveChanges();

		return NoContent();
	}

	/// <summary>
	/// Delete a lecture in a meetup by name.
	/// </summary>
	/// <param name="meetupName">Name for the given meetup ex:(dev-summit, girls who code)</param>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpDelete("{id}")]
	public ActionResult Delete(string meetupName, int id)
	{
		var meetup = _context.Meetups
			.Include(m => m.Lectures)
			.FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

		if (meetup is null)
			return NotFound($"A Meetup with the name: '{meetup}' is not found.");

		var lecture = meetup.Lectures.FirstOrDefault(l => l.Id == id);

		if (lecture is null)
			return NotFound($"A Lecture id of '{id}' is not a part of the meetup '{meetup.Name}'");

		_context.Lectures.Remove(lecture);
		_context.SaveChanges();

		return NoContent();
	}
}
