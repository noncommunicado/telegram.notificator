using Microsoft.Extensions.Hosting;
using Neftm.Telegram.Notificator;
using Neftm.Telegram.Notificator.Requests;

public class Program
{
	public static void Main(string[] args)
	{
		IHost host = CreateHostBuilder(args).Build();
		
		var repo = (INotificatorRepository)host.Services.GetService(typeof(INotificatorRepository))!;
		
		
		
		Console.ReadLine();
		
		host.Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureHostConfiguration(configuration =>
			{
			
			})
		.ConfigureServices((hostContext, services) =>
			{
				services.AddTelegramNotificator();
			});
}