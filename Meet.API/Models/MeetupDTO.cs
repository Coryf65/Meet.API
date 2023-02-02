namespace Meet.API.Models;

public class MeetupDTO
{
	public string Name { get; set; }
	public string Organizer { get; set; }
	public DateTime Date { get; set; }
	public bool IsPrivate { get; set; }
}
