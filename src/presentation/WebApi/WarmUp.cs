
using Persistence.Contexts;
using Serilog;

namespace WebApi;

public sealed class WarmUp
{
	public static async Task RunAsync(WebApplication app)
	{
		Log.Information("WarmUp...");
		using var scope = app.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
		await DatabaseInitializer.InitializeAsync(context);
	}
}