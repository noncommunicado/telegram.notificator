
namespace Domain.Models;

public class MessageModel
{
	public Guid Id { get; set; }
	
	/// <summary>
	/// Maximum Length 1500 symbols
	/// </summary>
	public string Text { get; set; }
	
	/// <summary>
	/// Is send without notification (push & sound)
	/// </summary>
	public bool DisableNotification { get; set; } = false;
}