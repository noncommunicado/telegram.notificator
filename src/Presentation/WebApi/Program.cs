using System.Globalization;
using System.Reflection;
using Application.Interfaces;
using FastEndpoints.ClientGen;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Persistence;
using TelegramBot;
using Neftm.Telegram.Notificator.Rabbit;
using Services;
using WebApi.Configuration;
using WebApi.Middlewares;

CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.AddJsonFile("appsettings.json", false, false)
	.AddJsonFile( $"appsettings.{builder.Environment.EnvironmentName}.json", true, false);

builder.ConfigureLogging();

builder.Services.AddFastEndpoints(o => {
	o.IncludeAbstractValidators = true;
	o.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All;
});

builder.Services.AddSwaggerDoc(
	maxEndpointVersion: 1,
	shortSchemaNames: true,
	removeEmptySchemas: true,
	settings: s => {
		s.DocumentName = "Release 1.0";
		s.Version = "v1.0";

		var clientDirectory = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly()!.Location).DirectoryName!, "Clients");
		if (!Directory.Exists(clientDirectory))
			Directory.CreateDirectory(clientDirectory);
		s.GenerateCSharpClient(
			settings: z => z.ClassName = "ApiClient",
			destination: Path.Combine(clientDirectory, "ClientCSharp.cs"));
		s.GenerateTypeScriptClient(
			settings: z => z.ClassName = "ApiClient",
			destination: Path.Combine(clientDirectory, "ClientJS.ts"));
	});

builder.Services.AddMainDbContext(builder.Configuration.GetConnectionString("Main")!);
builder.Services.AddTgBotSender(builder.Configuration.GetSection("Telegram").Get<TelegramBotOptions>()!);
builder.Services.AddMassTransitConfiguration(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Application.AssemblyInfo).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAutoMapper(typeof(Program), typeof(Domain.AssemblyInfo), typeof(Application.AssemblyInfo));
builder.Services.AddSingleton<IMessageCache, MessageCache>();
builder.Services.AddSingleton<ExceptionMiddleware>();

// quartz
builder.Services.ConfigureQuartz();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.UseFastEndpoints(c => {
	c.Endpoints.RoutePrefix = "api";
	c.Endpoints.ShortNames = true;
	c.Versioning.Prefix = "v";
	c.Versioning.PrependToRoute = true;
	c.Versioning.DefaultVersion = 1;
});
app.UseSwaggerGen();
app.UseMiddleware<ExceptionMiddleware>();

// set listen addresses
foreach (var host in app.Configuration.GetSection("ListenOn")!.Get<string[]>()!)
	app.Urls.Add(host);

app.Run();