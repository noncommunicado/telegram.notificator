using FluentValidation;

namespace WebApi.Endpoints.V1.Telegram.SendToChats;

public sealed class SendTelegramMessageToChatsRequest : SendTelegramMessageRequestBase
{
	public IEnumerable<long> Chats { get; set; }

	public SendTelegramMessageToChatsRequest()
	{
		
	}

	public SendTelegramMessageToChatsRequest(string text, bool isDisableNotification, IEnumerable<long> chats) : base(text, isDisableNotification)
	{
		this.Chats = chats;
	}
}


public class SendToChatsRequestValidator : SendRequestBaseValidator<SendTelegramMessageToChatsRequest>
{
	public SendToChatsRequestValidator() : base()
	{
		RuleFor(x => x.Chats)
			.NotEmpty().WithMessage("Как минимум один чат должен быть указан");
	}
}