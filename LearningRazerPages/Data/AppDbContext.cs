using LearningRazerPages.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningRazerPages.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Person> People { get; set; }
}
