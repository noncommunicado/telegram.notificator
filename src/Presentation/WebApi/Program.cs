using System.Globalization;
using Application.Interfaces;
using FastEndpoints.Swagger;
using FluentValidation;
using Persistence;
using TelegramBot;
using Neftm.Telegram.Notificator.Rabbit;
using Services;
using WebApi.Configuration;
using WebApi.Middlewares;

CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", false, false);
#if DEBUG
builder.Configuration.AddJsonFile( $"appsettings.{builder.Environment.EnvironmentName}.json", true, false);
#endif

builder.ConfigureLogging();

// api
builder.Services.AddFastEndpointsConfiguration(builder.Configuration);

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

app.UseAuthentication();
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