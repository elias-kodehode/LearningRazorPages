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


	public async Task OnGetAsync()
	{
		this.People = await db.People.ToListAsync();
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if(!ModelState.IsValid)
		{
			this.People = await db.People.ToListAsync();
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
