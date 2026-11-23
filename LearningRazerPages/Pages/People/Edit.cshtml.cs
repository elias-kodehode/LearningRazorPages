using System.ComponentModel.DataAnnotations;
using LearningRazerPages.Data;
using LearningRazerPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningRazerPages.Pages.People;

public class EditModel(AppDbContext db) : PageModel
{

	[BindProperty]
	public PersonInput Input { get; set; } = default!;


	public async Task<IActionResult> OnGetAsync(int id)
	{
		var person = await db.People.FindAsync([id]);

		if(person is null)
		{
			return NotFound();
		}


		this.Input = new()
		{
			Age = person.Age,
			FirstName = person.FirstName,
			LastName = person.LastName
		};

		return Page();
	}

	public async Task<IActionResult> OnPostAsync(int id)
	{
		if(!ModelState.IsValid)
			return Page();

		var person = await db.People.FindAsync(id);

		if(person is null)
			return NotFound();

		person.FirstName = Input.FirstName;
		person.LastName = Input.LastName;
		person.Age = Input.Age;

		await db.SaveChangesAsync();
		this.TempData["Success"] = "Person updated successfully!";
		return RedirectToPage("/People");
	}

	public async Task<IActionResult> OnPostDeleteAsync(int id)
	{
		var person = await db.People.FindAsync(id);

		if(person is null)
			return NotFound();

		db.People.Remove(person);
		await db.SaveChangesAsync();

		this.TempData["Success"] = "Person deleted successfully";
		return RedirectToPage("/People");
	}


	public class EditPersonInput
	{
		[Required, StringLength(50)]
		public required string FirstName { get; set; } = "";

		[Required, StringLength(50)]
		public required string LastName { get; set; } = "";
		[Range(0, 150)]
		public required int Age { get; set; }
	}

}
