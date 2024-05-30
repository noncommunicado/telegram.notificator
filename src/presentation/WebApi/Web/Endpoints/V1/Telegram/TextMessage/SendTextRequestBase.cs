using Domain.Models;
using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Telegram.TextMessage;


/// <summary>
///     Базовый запрос на отправку сообщения
/// </summary>
public abstract class SendTextRequestBase
{
	public SendTextRequestBase() { }

	public SendTextRequestBase(string text, bool isDisableNotification)
	{
		Message = new SendMessageRequest {
			Text = text, DisableNotification = isDisableNotification
		};
	}

	public SendMessageRequest Message { get; set; }
}

/// <summary>
///     Базовый валидатор запроса на отправку сообщения
/// </summary>
public abstract class SendTextRequestBaseValidator<T> : AbstractValidator<T> where T : SendTextRequestBase
{
	public SendTextRequestBaseValidator()
	{
		RuleFor(x => x.Message.Text).NotEmpty().NotNull().Length(1, 4096);
	}
}