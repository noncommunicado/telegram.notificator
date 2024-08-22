
using Bll.CQRS.Commands.TelegramMessaging.Immediate;
using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Telegram.Immediate.SendToChats;

public sealed class Validator : AbstractValidator<SendMessageImmediatelyCommand>
{
	public Validator()
	{
		RuleFor(x => x.Text).MaximumLength(4096);
		RuleFor(x => x.Text).NotEmpty();
	}
}