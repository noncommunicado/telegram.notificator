namespace Neftm.Telegram.Notificator.Requests;

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
