using LearningRazerPages.Data;
using LearningRazerPages.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("db"));

var app = builder.Build();
using(var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider
		.GetRequiredService<AppDbContext>();

	await db.Database.EnsureCreatedAsync();

	if(!await db.People.AnyAsync())
	{
		db.People.AddRange(
			new Person
			{
				FirstName = "John",
				LastName = "Doe",
				Age = 25
			},
			new Person
			{
				FirstName = "Jane",
				LastName = "Smith",
				Age = 30
			}
		);

		await db.SaveChangesAsync();
	}
}
if(!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/404");
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
