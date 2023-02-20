using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddMainDbContext(this IServiceCollection serviceCollection, string connectionString)
	{
		serviceCollection.AddDbContext<IMainDbContext, MainDbContext>(o => o.UseNpgsql(connectionString));
		return serviceCollection;
	}
}