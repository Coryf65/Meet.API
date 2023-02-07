namespace Meet.API.Models;

public class PageResults<T>
{
	public List<T> Items { get; set; }
	public int TotalPages { get; set; }
	public int ItemsFrom { get; set; }
	public int ItemsTo { get; set; }
	public int TotalItemsCount { get; set; }

	public PageResults(List<T> items, int totalCount, int pageNumber, int pageSize)
	{
		Items = items;
		TotalItemsCount = totalCount;
		ItemsFrom = pageSize * (pageNumber - 1) + 1;
		ItemsTo = ItemsFrom + pageSize - 1;
		TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
	}
}
