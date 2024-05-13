using Domain.Models;
using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Telegram;

/// <summary>
///     Базовый запрос на отправку сообщения
/// </summary>
public abstract class SendTelegramMessageRequestBase
{
	public SendTelegramMessageRequestBase() { }

	public SendTelegramMessageRequestBase(string text, bool isDisableNotification)
	{
		Message = new MessageBase {
			Text = text, DisableNotification = isDisableNotification
		};
	}

	public virtual MessageBase Message { get; set; }
}

/// <summary>
///     Базовый валидатор запроса на отправку сообщения
/// </summary>
public abstract class SendRequestBaseValidator<T> : AbstractValidator<T> where T : SendTelegramMessageRequestBase
{
	public SendRequestBaseValidator()
	{
		RuleFor(x => x.Message.Text).NotEmpty().NotNull().Length(1, 1500);
	}
}