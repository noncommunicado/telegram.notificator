using Domain.Interfaces;
using MediatR;
using Serilog;
using Telegram.Bot;

namespace Bll.CQRS.Commands.TelegramMessaging.Immediate;

public sealed record DeleteMessageImmediatelyCommand(long ChatId, int MessageId) : IRequest;

public sealed class DeleteMessageImmediatelyCommandHandler : IRequestHandler<DeleteMessageImmediatelyCommand>
{
	private readonly IBotHostedService _botService;
	public DeleteMessageImmediatelyCommandHandler(IBotHostedService botService)
	{
		_botService = botService;
	}
	public async Task Handle(DeleteMessageImmediatelyCommand request, CancellationToken ct)
	{
		await _botService.BotClient.DeleteMessageAsync(request.ChatId, request.MessageId, ct);
	}
}