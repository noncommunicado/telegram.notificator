using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Neftm.Telegram.Notificator;

public static class DependencyInjection
{
	public static IServiceCollection AddTelegramNotificator(this IServiceCollection services, string baseUrl = "http://notify.neftm.local")
	{
		services
			.AddHttpClient(Constants.HttpClientName, client =>
			{
				client.BaseAddress = new Uri(baseUrl);
				client.Timeout = TimeSpan.FromSeconds(10);
			})
			.SetHandlerLifetime(TimeSpan.FromMinutes(5))
			.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
				.OrResult(msg => (int)msg.StatusCode >= 400)
				.WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
		
		services.AddSingleton<INotificatorRepository, NotificatorRepository>();
		return services;
	} 
}