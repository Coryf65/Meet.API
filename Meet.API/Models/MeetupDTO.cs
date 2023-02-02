using System.ComponentModel.DataAnnotations;

namespace Meet.API.Models;

public class MeetupDTO
{
	// make the name required and a min length of 3 using attributes
	[Required]
	[MinLength(3)]
	public string Name { get; set; }
	public string Organizer { get; set; }
	public DateTime Date { get; set; }
	public bool IsPrivate { get; set; }
}
