using AutoMapper;
using Meet.API.Data;
using Meet.API.Entities;
using Meet.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.API.Controllers;

[Route("api/meetup/{meetupName}/lecture")]
public class LectureController : Controller
{
	private readonly MeetupContext _context;
	private readonly IMapper _mapper;

	public LectureController(MeetupContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

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
			return NotFound($"A Lecture with the name: '{meetup}' is not found.");

		var lecture = _mapper.Map<Lecture>(model);

		meetup.Lectures.Add(lecture);
		_context.SaveChanges();

		return Created($"api/meetup/{meetupName}", null);
	}
}
