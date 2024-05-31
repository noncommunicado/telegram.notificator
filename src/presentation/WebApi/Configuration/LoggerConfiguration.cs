using System.Reflection;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using WebApi.Helpers;

namespace WebApi.Configuration;

internal static class LoggerConfiguration
{
	internal static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
	{
		var config = builder.Configuration.GetRequiredSection("Elastic").Get<LogConfig>()!;
		var elasticTemplateName = ElasticsearchHelper.GetTemplateName(config);
		builder.Logging.ClearProviders();

		builder.Host.UseSerilog((_, _, configuration) => {
			configuration
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails()
				.WriteTo.Logger(c =>
					c.WriteTo.File(Path.Combine("logs", "_.log"),
						rollingInterval: RollingInterval.Day,
						restrictedToMinimumLevel: LogEventLevel.Error)
				)
				.WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Fatal)
				.WriteTo.Elasticsearch(
					new ElasticsearchSinkOptions(new Uri(config.ElasticUri)) {
						AutoRegisterTemplate = true,
						OverwriteTemplate = true,
						TemplateName = elasticTemplateName,
						IndexFormat = $"{elasticTemplateName}-{DateTime.UtcNow:yyyy-MM}",
						MinimumLogEventLevel = LogEventLevel.Information
					});
		});
		return builder;
	}

	internal static WebApplication UseHttpRequestsLogging(this WebApplication app)
	{
		app.UseSerilogRequestLogging(options => {
			options.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
				var ipHeader = httpContext.Request.Headers.FirstOrDefault(x => x.Key.Equals("X-Real-IP"));
				var forwardedHeader = httpContext.Request.Headers.FirstOrDefault(x => x.Key.Equals("X-Forwarded-For"));

				diagnosticContext.Set("RemoteIpAddress", ipHeader.Value.Count > 0
					? ipHeader.Value[0]
					: httpContext.Request.HttpContext.Connection.RemoteIpAddress);
				diagnosticContext.Set("Forwarded",
					forwardedHeader.Value.Count > 0 ? forwardedHeader.Value[0] : "[none]");
				diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme.ToUpper());
				diagnosticContext.Set("ContentLength", httpContext.Request.ContentLength ?? 0);
				diagnosticContext.Set("ContentType", httpContext.Request.ContentType ?? "[none]");
				diagnosticContext.Set("HeadersCount", httpContext.Request.Headers.Count);
				diagnosticContext.Set("CookiesCount", httpContext.Request.Cookies.Count);
				diagnosticContext.Set("TraceIdentifier", httpContext.TraceIdentifier);
			};
			options.IncludeQueryInRequestPath = true;
		});
		return app;
	}
}