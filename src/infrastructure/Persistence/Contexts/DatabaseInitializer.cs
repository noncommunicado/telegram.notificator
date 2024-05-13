using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class DatabaseInitializer
{
	public static async Task InitializeAsync(MainDbContext context)
	{
		await context.Database.MigrateAsync();
	}
}