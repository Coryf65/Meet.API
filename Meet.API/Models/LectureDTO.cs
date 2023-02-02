using System.ComponentModel.DataAnnotations;

namespace Meet.API.Models;

public class LectureDTO
{
	[Required]
	[MinLength(5)]
	public string Author { get; set; } = string.Empty;
	[Required]
	[MinLength(5)]
	public string Topic { get; set; } = string.Empty;
	[Required]
	public string Description { get; set; } = string.Empty;
}
