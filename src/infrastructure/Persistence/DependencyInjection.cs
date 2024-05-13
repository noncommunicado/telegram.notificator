using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddMainDbContext(this IServiceCollection serviceCollection,
		string connectionString)
	{
		serviceCollection.AddDbContext<MainDbContext>(o
			=> o.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
		return serviceCollection;
	}
	
}