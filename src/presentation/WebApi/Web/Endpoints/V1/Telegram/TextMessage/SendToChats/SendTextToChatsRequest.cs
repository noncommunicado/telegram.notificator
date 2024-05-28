using Bll.CQRS.Commands.TelegramMessaging;
using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Telegram.TextMessage.SendToChats;

public sealed class SendTextToChatsRequest : SendTextRequestBase
{
	public SendTextToChatsRequest() { }

	public SendTextToChatsRequest(string text, bool isDisableNotification, IEnumerable<EnqueueChatsDto> chats) : base(
		text, isDisableNotification)
	{
		Chats = chats;
	}

	public IEnumerable<EnqueueChatsDto> Chats { get; set; }
}

public class SendTextToChatsRequestValidator : SendTextRequestBaseValidator<SendTextToChatsRequest>
{
	public SendTextToChatsRequestValidator()
	{
		RuleFor(x => x.Chats)
			.NotEmpty().WithMessage("Как минимум один чат должен быть указан");
	}
}