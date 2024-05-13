using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Telegram.SendToChats;

public sealed class SendTelegramMessageToChatsRequest : SendTelegramMessageRequestBase
{
	public SendTelegramMessageToChatsRequest() { }

	public SendTelegramMessageToChatsRequest(string text, bool isDisableNotification, IEnumerable<long> chats) : base(
		text, isDisableNotification)
	{
		Chats = chats;
	}

	public IEnumerable<long> Chats { get; set; }
}

public class SendToChatsRequestValidator : SendRequestBaseValidator<SendTelegramMessageToChatsRequest>
{
	public SendToChatsRequestValidator()
	{
		RuleFor(x => x.Chats)
			.NotEmpty().WithMessage("Как минимум один чат должен быть указан");
	}
}