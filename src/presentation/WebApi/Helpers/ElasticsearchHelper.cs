
namespace WebApi.Helpers;

public sealed class LogConfig
{
	public string ElasticIndex { get; set; } = null!;
	public string ElasticUri { get; set; } = null!;
}
public sealed class ElasticsearchHelper
{
	
	public static string GetTemplateName(LogConfig config)
	{
		if (!string.IsNullOrWhiteSpace(config.ElasticIndex))
			return config.ElasticIndex; // preset template name
		var cleanedAssembly = typeof(Program).Assembly.GetName().Name!.ToLower().Replace(".", "-");
		var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower()?.Trim() ?? "unknown";
		return $"{cleanedAssembly}_{envName}";
	}

}