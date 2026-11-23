
namespace LearningRazerPages.Models;

public class PaginationModel
{
	public int CurrentPage { get; set; }
	public int TotalPages { get; set; }
	public int TotalCount { get; set; }
	public string PageName { get; set; } = "";

	// Query parameters to preserve
	public Dictionary<string, string?> RouteValues { get; set; } = [];

	public bool HasPreviousPage => CurrentPage > 1;

	public bool HasNextPage => CurrentPage < TotalPages;
}
