using System.Globalization;
using System.Reflection;
using Application.Interfaces;
using FluentValidation;
using Persistence;
using TelegramBot;
using MediatR;
using Neftm.Telegram.Notificator.Rabbit;
using Quartz;
using Services;
using WebApi.Configuration;
using WebApi.Jobs;
using WebApi.Middlewares;

CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.AddJsonFile("appsettings.json", false, false)
	.AddJsonFile( $"appsettings.{builder.Environment.EnvironmentName}.json", true, false);

builder.ConfigureLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer().AddCors();
builder.Services.AddSwaggerGen(opt =>
{
	opt.IncludeXmlComments(
		Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, $"{Assembly.GetExecutingAssembly()!.GetName().Name}.xml"), true); 
	opt.EnableAnnotations();
});

builder.Services.AddMainDbContext(builder.Configuration.GetConnectionString("Main")!);
builder.Services.AddTgBotSender(builder.Configuration.GetSection("Telegram").Get<TelegramBotOptions>()!);
builder.Services.AddMassTransitConfiguration(builder.Configuration);
builder.Services.AddMediatR(new [] {typeof(Application.AssemblyInfo).Assembly});
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAutoMapper(typeof(Program), typeof(Domain.AssemblyInfo), typeof(Application.AssemblyInfo));
builder.Services.AddSingleton<IMessageCache, MessageCache>();
builder.Services.AddSingleton<ExceptionMiddleware>();

// quartz
builder.Services.AddQuartz(q =>
{
	q.UseMicrosoftDependencyInjectionJobFactory();
	var rmMsgJobKey = new JobKey(nameof(RemoveOldMessagesJob));
	q.AddJob<RemoveOldMessagesJob>(opts => opts.WithIdentity(rmMsgJobKey));
	q.AddTrigger(opt => opt.ForJob(rmMsgJobKey)
		.WithIdentity(nameof(RemoveOldMessagesJob)).WithSimpleSchedule(b => b.WithIntervalInMinutes(60))
	);
});
builder.Services.AddQuartzHostedService(options => {
	options.WaitForJobsToComplete = true;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();

// set listen addresses
foreach (var host in app.Configuration.GetSection("ListenOn")!.Get<string[]>()!)
	app.Urls.Add(host);

app.Run();