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

	[HttpGet]
	public ActionResult<List<MeetupDetailsDTO>> Get()
	{
		List<Meetup> meetups = _context.Meetups.Include(m => m.Location).ToList();
		List<MeetupDetailsDTO> meetupsDtos = _mapper.Map<List<MeetupDetailsDTO>>(meetups);

		return Ok(meetupsDtos);
	}
}