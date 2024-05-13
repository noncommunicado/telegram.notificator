using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot;

public static class DependencyInjection
{
	public static IServiceCollection AddTgBotSender(this IServiceCollection services, TelegramBotOptions options)
	{
		services.AddSingleton(options);
		services.AddSingleton<IBotHostedService, BotService>();
		services.AddHostedService<BotService>();
		return services;
	}
}