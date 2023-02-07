namespace Meet.API.Models;

public class MeetupQuery
{
	public string? SearchPhrase { get; set; }
	public int PageSize { get; set; }
	public int PageNumber { get; set; }
}
