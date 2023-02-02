using Meet.API.Entities;

namespace Meet.API.Models;

public class MeetupDetailsDTO
{
	public string Name { get; set; }
	public string Organizer { get; set; }
	public DateTime Date { get; set; }
	public bool IsPrivate { get; set; }

	public string City { get; set; }
	public string Street { get; set; }
	public string PostCode { get; set; }
	public List<LectureDTO> Lectures { get; set; }
}
