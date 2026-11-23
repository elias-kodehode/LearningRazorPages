using System.ComponentModel.DataAnnotations;

namespace LearningRazerPages.Models;

public class PersonInput
{
	[Required]
	[StringLength(100)]
	public string FirstName { get; set; } = "";

	[Required]
	[StringLength(100)]
	public string LastName { get; set; } = "";

	[Range(0, 150)]
	public int Age { get; set; }
}
