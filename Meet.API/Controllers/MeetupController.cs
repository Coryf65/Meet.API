using AutoMapper;
using Meet.API.Entities;
using Meet.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
		var meetup = _mapper.Map<Meetup>(model);

		_context.Meetups.Add(meetup);
		_context.SaveChanges();

		var key = meetup.Name.Replace(" ", "-").ToLower();

		return Created($"api/meetup/{key}", null);
	}
}