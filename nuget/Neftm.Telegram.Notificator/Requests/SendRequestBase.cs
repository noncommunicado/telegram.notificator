namespace Neftm.Telegram.Notificator.Requests;

/// <summary>
///     Базовый запрос на отправку сообщения
/// </summary>
public abstract class SendRequestBase
{
	public SendRequestBase() { }

	public SendRequestBase(string text, bool isDisableNotification)
	{
		Message = new MessageBase {
			Text = text, DisableNotification = isDisableNotification
		};
	}

	public virtual MessageBase Message { get; set; }
}