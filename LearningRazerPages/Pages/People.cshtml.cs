using LearningRazerPages.Data;
using LearningRazerPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LearningRazerPages.Pages;

public class PeopleModel(AppDbContext db) : PageModel
{
	public List<Person> People { get; private set; } = [];

	[BindProperty]
	public Person NewPerson { get; set; } = new();

	[BindProperty(SupportsGet = true)]
	public string? Search { get; set; }

	[BindProperty(SupportsGet = true)]
	public int? MinAge { get; set; }

	[BindProperty(SupportsGet = true)]
	public int? MaxAge { get; set; }

	[BindProperty(SupportsGet = true)]
	public int PageNumber { get; set; } = 1;

	public int TotalPages { get; private set; }
	public int TotalCount { get; private set; }

	private const int PageSize = 5;


	public Task OnGetAsync() => LoadPeopleAsync();
	private async Task LoadPeopleAsync()
	{
		var query = db.People.AsNoTracking();

		if(!string.IsNullOrEmpty(Search))
		{
			var search = Search.Trim();

			query = query.Where(x => ($"{x.FirstName} {x.LastName}").ToLower().Contains(search.ToLower()));
			//query = query.Where(x => x.FirstName.ToLower().Contains(search.ToLower()) || x.LastName.ToLower().Contains(search.ToLower()));
		}

		if(MinAge.HasValue)
			query = query.Where(x => x.Age >= MinAge.Value);

		if(MaxAge.HasValue)
			query = query.Where(x => x.Age <= MaxAge.Value);

		TotalCount = await query.CountAsync();

		TotalPages = Math.Max(1, (int)Math.Ceiling((double)TotalCount / PageSize));

		PageNumber = Math.Clamp(PageNumber, 1, TotalPages);

		this.People = await query.OrderBy(x => x.Id)
			.Skip((PageNumber - 1) * PageSize)
			.Take(PageSize)
			.ToListAsync();
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if(!ModelState.IsValid)
		{
			await LoadPeopleAsync();
			//this.People = await db.People.ToListAsync();
			return Page();
		}
		db.People.Add(NewPerson);
		await db.SaveChangesAsync();
		return RedirectToPage();
	}

	public async Task<IActionResult> OnPostDeleteAsync(int id)
	{
		var person = await db.People.FindAsync([id]);

		if(person is null)
			return NotFound();

		db.People.Remove(person);
		await db.SaveChangesAsync();
		return RedirectToPage();
	}

}
