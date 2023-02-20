
using Domain.Models;
using FluentValidation;

namespace WebApi.Http.Requests;

/// <summary>
/// Базовый запрос на отправку сообщения
/// </summary>
public abstract class SendRequestBase
{
	public SendRequestBase()
	{
		
	}

	public SendRequestBase(string text, bool isDisableNotification)
	{
		Message = new MessageBase
		{
			Text = text, DisableNotification = isDisableNotification
		};
	}
	public virtual MessageBase Message { get; set; }
}


/// <summary>
/// Базовый валидатор запроса на отправку сообщения
/// </summary>
public abstract class SendRequestBaseValidator<T> : AbstractValidator<T> where T : SendRequestBase
{
	public SendRequestBaseValidator()
	{
		RuleFor(x => x.Message.Text).NotEmpty().NotNull().Length(1, 1500);
	}
}