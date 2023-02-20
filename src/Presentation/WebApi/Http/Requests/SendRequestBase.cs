
using Domain.Models;
using FluentValidation;

namespace WebApi.Http.Requests;

/// <summary>
/// Базовый запрос на отправку сообщения
/// </summary>
public abstract class SendRequestBase
{
	public virtual MessageModel Message { get; set; }
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