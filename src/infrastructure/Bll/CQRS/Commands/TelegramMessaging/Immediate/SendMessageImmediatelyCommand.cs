using Domain.Interfaces;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bll.CQRS.Commands.TelegramMessaging.Immediate;

public sealed record SendMessageImmediatelyCommand(long ChatId, string Text, bool DisableNotification, int? ThreadId = null) 
	 : IRequest<Message>;

public sealed class SendMessageImmediatelyCommandHandler : IRequestHandler<SendMessageImmediatelyCommand, Message>
{
	private readonly IBotHostedService _botService;
	public SendMessageImmediatelyCommandHandler(IBotHostedService botService)
	{
		_botService = botService;
	}
	public async Task<Message> Handle(SendMessageImmediatelyCommand request, CancellationToken cancellationToken)
	{
		return await _botService.BotClient.SendTextMessageAsync(
			request.ChatId, request.Text,
			messageThreadId: request.ThreadId,
			parseMode: ParseMode.Html,
			disableNotification: request.DisableNotification);
	}
}