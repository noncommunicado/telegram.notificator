namespace WebApi.Configuration;

public static class AppSettingsConfiguration
{
	public static WebApplicationBuilder ConfigureAppSettings(this WebApplicationBuilder builder)
	{
		builder.Configuration.AddJsonFile(Path.Combine("appsettings", "appsettings.json"), false);
		builder.Configuration.AddJsonFile(Path.Combine("appsettings", "appsettings.Development.json"), true);
		
		return builder;
	}
}