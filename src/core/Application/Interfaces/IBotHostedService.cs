using Microsoft.Extensions.Hosting;

namespace Application.Interfaces;

public interface IBotHostedService : IBotService, IHostedService
{
	
}