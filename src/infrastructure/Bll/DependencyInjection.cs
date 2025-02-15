using Bll.Mq.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Bll;

public static class DependencyInjection
{
	public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddOptions<RabbitMqTransportOptions>()
			.Configure(options => { configuration.Bind("Rabbit", options); });

		services.AddMassTransit(x => {
			x.AddConsumer<SendTelegramNotifyConsumer>(cfg => {
				cfg.UseMessageRetry(x => {
					x.Exponential(int.MaxValue, TimeSpan.FromSeconds(3), TimeSpan.FromMinutes(30),
						TimeSpan.FromSeconds(10));
				});
			});

			x.UsingRabbitMq((context, cfg) => {
				cfg.Durable = true;
				cfg.ConfigureEndpoints(context);
			});
		});
		return services;
	}
}