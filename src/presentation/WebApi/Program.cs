using System.Globalization;
using Bll;
using Domain.Interfaces;
using FastEndpoints.Swagger;
using FluentValidation;
using Persistence;
using Serilog;
using Services;
using TelegramBot;
using WebApi;
using WebApi.Configuration;
using WebApi.Middlewares;

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
	builder.Services.AddSingleton<IFileService, FileService>(_ => new FileService(builder.Environment.WebRootPath));

// quartz
	builder.Services.ConfigureQuartz();

	var app = builder.Build();

	app.UseAuthentication();
	app.UseAuthorization();

	app.UseFastEndpoints(c => {
		c.Endpoints.RoutePrefix = "api";
		c.Versioning.Prefix = "v";
		c.Versioning.PrependToRoute = true;
		c.Versioning.DefaultVersion = 1;
	}).UseSwaggerGen(settings => { });

	app.UseMiddleware<ExceptionMiddleware>();

	await WarmUp.RunAsync(app);
	Log.Information("Application Start");
	await app.RunAsync();
}
catch (Exception ex) {
	Log.Fatal(ex, "Fatal error");
}
finally {
	await Log.CloseAndFlushAsync();
}