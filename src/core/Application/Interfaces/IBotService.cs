
using Domain.BotService;
using Domain.Models;

namespace Application.Interfaces;

public interface IBotService
{
	/// <summary>
	/// Send text request 
	/// </summary>
	Task SendAsync(SendMessageModel request, CancellationToken ct);
}