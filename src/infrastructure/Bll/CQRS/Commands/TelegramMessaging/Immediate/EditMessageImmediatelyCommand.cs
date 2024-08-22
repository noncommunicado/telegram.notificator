using Domain.Interfaces;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bll.CQRS.Commands.TelegramMessaging.Immediate;

public sealed record EditMessageImmediatelyCommand(long ChatId, int MessageId, string Text, int? ThreadId = null) 
	 : IRequest<Message>;

public sealed class EditMessageImmediatelyCommandHandler : IRequestHandler<EditMessageImmediatelyCommand, Message>
{
	private readonly IBotHostedService _botService;
	public EditMessageImmediatelyCommandHandler(IBotHostedService botService)
	{
		_botService = botService;
	}
	public async Task<Message> Handle(EditMessageImmediatelyCommand r, CancellationToken ct)
	{
		return await _botService.BotClient.EditMessageTextAsync(
			r.ChatId, r.MessageId,
			r.Text,
			parseMode: ParseMode.Html,
			cancellationToken: ct
		);
	}
}