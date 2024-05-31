using Domain.BotService;

namespace Domain.Interfaces;

public interface IBotService
{
	/// <summary>
	///     Send text request
	/// </summary>
	Task SendAsync(SendTelegramMessageCommand request, CancellationToken ct = default);
}