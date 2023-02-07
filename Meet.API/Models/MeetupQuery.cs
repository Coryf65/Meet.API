namespace Meet.API.Models;

public class MeetupQuery
{
	/// <summary>
	/// What string name to lookup
	/// </summary>
	public string? SearchPhrase { get; set; }
	public int PageSize { get; set; }
	public int PageNumber { get; set; }
	/// <summary>
	/// Name of Column to Sort by
	/// </summary>
	public string? SortBy { get; set; }
	public SortDirection SortDirection { get; set; }
}
