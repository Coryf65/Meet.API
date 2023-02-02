using Meet.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Meet.API.Data;

[Route("api/meetup")]
public class MeetupController : Controller
{
	private readonly MeetupContext _context;

	public MeetupController(MeetupContext context)
	{
		_context = context;
	}

	[HttpGet]
	public ActionResult<List<Meetup>> Get()
	{
		var meetups = _context.Meetups.ToList();

		return Ok(meetups);
	}
}