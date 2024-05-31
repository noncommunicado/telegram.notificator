using Microsoft.Extensions.Hosting;

namespace Domain.Interfaces;

public interface IBotHostedService : IBotService, IHostedService { }