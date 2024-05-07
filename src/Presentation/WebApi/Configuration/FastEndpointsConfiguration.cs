using System.Reflection;
using FastEndpoints.ClientGen;
using FastEndpoints.Swagger;
using WebApi.Settings;

namespace WebApi.Configuration;

public static class FastEndpointsConfiguration
{
	public static IServiceCollection AddFastEndpointsConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtSettings>(configuration.GetSection("Authentication:Jwt"));
		
		services.AddFastEndpoints(o => {
			o.IncludeAbstractValidators = true;
		});
		services.SwaggerDocument(x => {
			x.MaxEndpointVersion = 1;
			x.ShortSchemaNames = true;
			x.RemoveEmptyRequestSchema = true;
		});
		
		return services;
	}
}