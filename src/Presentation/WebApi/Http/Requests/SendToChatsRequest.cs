using FluentValidation;

namespace WebApi.Http.Requests;

public sealed class SendToChatsRequest : SendRequestBase
{
	public IEnumerable<long> Chats { get; set; }

	public SendToChatsRequest()
	{
		
	}

	public SendToChatsRequest(string text, bool isDisableNotification, IEnumerable<long> chats) : base(text, isDisableNotification)
	{
		this.Chats = chats;
	}
}


public class SendToChatsRequestValidator : SendRequestBaseValidator<SendToChatsRequest>
{
	public SendToChatsRequestValidator() : base()
	{
		RuleFor(x => x.Chats)
			.NotEmpty().WithMessage("Как минимум один чат должен быть указан");
	}
}