
namespace Neftm.Telegram.Notificator.Requests;

public class MessageBase
{
	/// <summary>
	/// Maximum Length 1500 symbols
	/// </summary>
	public virtual string Text { get; set; }
	
	/// <summary>
	/// Is send without notification (push & sound)
	/// </summary>
	public virtual bool DisableNotification { get; set; } = false;
}