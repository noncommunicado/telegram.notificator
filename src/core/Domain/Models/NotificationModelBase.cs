
namespace Domain.Models;

public abstract class NotificationModelBase
{
	public NotificationModelBase()
	{
		
	}

	public NotificationModelBase(IEnumerable<long> chats, MessageModel message)
	{
		Chats = chats;
		Message = message;
	}

	public IEnumerable<long> Chats { get; set; }
	public virtual MessageModel Message { get; set; }
}