using FluentValidation;

namespace WebApi.Http.Requests;

public sealed class SendToChatsRequest : SendRequestBase
{
	public IEnumerable<long> Chats { get; set; }
}


public class SendToChatsRequestValidator : SendRequestBaseValidator<SendToChatsRequest>
{
	public SendToChatsRequestValidator() : base()
	{
		RuleFor(x => x.Chats)
			.NotEmpty().WithMessage("Как минимум один чат должен быть указан");
	}
}