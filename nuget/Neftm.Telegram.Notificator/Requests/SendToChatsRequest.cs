namespace Neftm.Telegram.Notificator.Requests;

public sealed class SendToChatsRequest : SendRequestBase
{
	public SendToChatsRequest() { }

	public SendToChatsRequest(string text, bool isDisableNotification, IEnumerable<long> chats) : base(text,
		isDisableNotification)
	{
		Chats = chats;
	}

	public IEnumerable<long> Chats { get; set; }
}