using Domain.BotService;

namespace Application.Interfaces;

public interface IBotService
{
	/// <summary>
	///     Send text request
	/// </summary>
	Task SendAsync(SendMessageModel request, CancellationToken ct);
}