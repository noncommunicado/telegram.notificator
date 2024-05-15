using Application.Interfaces;
using Domain.BotService;
using MediatR;
using Serilog;
using Telegram.Bot.Exceptions;

namespace Bll.CQRS.Commands.TelegramMessaging;

public sealed record SendMessageCommand(long ChatId, int ThreadId, Guid MessageId) : IRequest;

public sealed class SendNotifyCommandHandler : IRequestHandler<SendMessageCommand>
{
	private readonly IBotHostedService _botService;
	private readonly IMessageCache _messageCache;

	public SendNotifyCommandHandler(
		IBotHostedService botService, IMessageCache messageCache)
	{
		_botService = botService;
		_messageCache = messageCache;
	}

	public async Task Handle(SendMessageCommand request, CancellationToken ct)
	{
		var msg = await _messageCache.GetById(request.MessageId, ct);
		Log.Information("Sending Telegram message {MessageId} to chat {ChatId}, thread {ThreadId}", request.MessageId, request.ChatId, request.ThreadId);
		try {
			await _botService.SendAsync(new SendMessageModel(request.ChatId, request.ThreadId, msg), ct);
		}
		catch (ApiRequestException arex) when (arex.ErrorCode == 400) {
			// dont throw - because nothing to do with it - swallow
			Log.Error(arex, "Telegram-Bot don't has chat with this chatid");
		}
		catch (ApiRequestException arex) when (arex.ErrorCode is 420 or >= 500) {
			// must be thrown for RMQ retry
			Log.Error(arex, "Telegram API is currently unavailable or overloaded");
			throw;
		}
	}
}