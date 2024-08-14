using System.Globalization;
using System.Reflection;
using Bll;
using FastEndpoints.ClientGen.Kiota;
using FastEndpoints.Swagger;
using FluentValidation;
using Kiota.Builder;
using KutCode.Security.Ldap.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Persistence;
using Serilog;
using Services;
using TelegramBot;
using WebApi;
using WebApi.Configuration;
using WebApi.Middlewares;
using WebApi.Services;

CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");

try {
	var builder = WebApplication.CreateBuilder(args);

	builder.ConfigureAppSettings().AddFastEndpointsConfiguration().ConfigureLogging();

// api
	builder.Services.AddMainDbContext(builder.Configuration.GetConnectionString("Main")!);
	builder.Services.AddTgBotSender(builder.Configuration.GetSection("Telegram").Get<TelegramBotOptions>()!);
	builder.Services.AddMassTransitConfiguration(builder.Configuration);
	builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Bll.AssemblyInfo).Assembly));
	builder.Services.AddValidatorsFromAssemblyContaining<Program>();
	builder.Services.AddAutoMapper(typeof(Program), typeof(Domain.AssemblyInfo));
	builder.Services.AddSingleton<ExceptionMiddleware>();
	builder.Services.AddSingleton<JwtTokenGenerator>();
	builder.Services.AddSingleton<IFileService, FileService>(_ => new FileService(Path.Combine(builder.Environment.WebRootPath, "files")));
	builder.Services.AddKutCodeLdapRepository(builder.Configuration.GetRequiredSection("Ldap"), ServiceLifetime.Scoped);
	builder.Services.AddCors();
	var webRootPath = Path.Combine(new FileInfo(Assembly.GetEntryAssembly()!.Location).Directory!.FullName, "wwwroot");
	builder.Services.AddSpaStaticFiles(x => x.RootPath = webRootPath);

// quartz
	builder.Services.ConfigureQuartz();

	var app = builder.Build();

	app.UseCors(c => c.SetIsOriginAllowed(_ => true)
		.AllowCredentials().AllowAnyHeader().AllowAnyMethod());
	app.UseAuthentication();
	app.UseAuthorization();

	app.MapApiClientEndpoint("/api/v1/generate-client/csharp", c => {
		c.SwaggerDocumentName = "v1"; 
		c.Language = GenerationLanguage.CSharp;
		c.ClientNamespaceName = "Neftm.Notificator";
		c.ClientClassName = "NotificatorRepository";
	},
	o => //endpoint customization settings
	{
		o.CacheOutput(p => p.Expire(TimeSpan.FromDays(365))); //cache the zip
		//o.ExcludeFromDescription(); //hides this endpoint from swagger docs
	});
	app.MapApiClientEndpoint("/api/v1/generate-client/typescript", c => {
		c.SwaggerDocumentName = "v1"; 
		c.Language = GenerationLanguage.TypeScript;
		c.ClientNamespaceName = "notificator";
		c.ClientClassName = "NotificatorRepository";
	},
	o => //endpoint customization settings
	{
		o.CacheOutput(p => p.Expire(TimeSpan.FromDays(365))); //cache the zip
		//o.ExcludeFromDescription(); //hides this endpoint from swagger docs
	});
	app.UseFastEndpoints(c => {
		c.Endpoints.RoutePrefix = "api";
		c.Versioning.Prefix = "v";
		c.Versioning.PrependToRoute = true;
		c.Versioning.DefaultVersion = 1;
	}).UseSwaggerGen(settings => {
		
	});
	app.UseSpaStaticFiles();
	app.UseSpa(c => {
		c.Options.DefaultPageStaticFileOptions = new StaticFileOptions {
			FileProvider = new PhysicalFileProvider(webRootPath),
		};
	});
	app.MapFallbackToFile("index.html");
	app.UseMiddleware<ExceptionMiddleware>();

	await WarmUp.RunAsync(app);
	Log.Information("Application Start");
	await app.RunAsync();
}
catch (Exception ex) {
	Console.WriteLine(ex.Message);
	Log.Fatal(ex, "Fatal error");
}
finally {
	await Log.CloseAndFlushAsync();
}