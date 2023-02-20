using System.Reflection;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace WebApi.Configuration;

internal static class LoggerConfiguration
{
	internal static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
	{
		var basePath = new FileInfo(Assembly.GetEntryAssembly()!.Location).Directory!.FullName;
		builder.Logging.ClearProviders();
		
		builder.Host.UseSerilog((context, services, configuration) =>
		{
			configuration
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails()
				.WriteTo.Logger(c => c
					.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
					.Filter.ByExcluding(le => le.Properties.GetValueOrDefault("SourceContext") is ScalarValue sv
					                          && ((bool) sv.Value?.ToString()?.StartsWith("Microsoft") ||
					                              (bool) sv.Value?.ToString()?.StartsWith("System")))
					.WriteTo.File(Path.Combine(basePath, "Logs", "Info", "_.log"), rollingInterval: RollingInterval.Day)
				)
				.WriteTo.Logger(c => c
					.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
					.WriteTo.File(Path.Combine(basePath, "Logs", "Warning", "_.log"),
						rollingInterval: RollingInterval.Day)
				)
				.WriteTo.Logger(c => c
					.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error || e.Level == LogEventLevel.Fatal)
					.WriteTo.File(Path.Combine(basePath, "Logs", "Error", "_.log"),
						rollingInterval: RollingInterval.Day)
					.WriteTo.Console()
				);
		});
		return builder;
	}

	internal static WebApplication UseHttpRequestsLogging(this WebApplication app)
	{
		app.UseSerilogRequestLogging(options =>
		{
			options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
			{
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