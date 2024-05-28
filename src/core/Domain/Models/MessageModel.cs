using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class MessageModel : MessageBase
{
	public Guid Id { get; set; }
}

public class MessageBase
{
	/// <summary>
	///     Maximum Length 4096 symbols for text messages and 200 for content caption
	/// </summary>
	public virtual string Text { get; set; }

	/// <summary>
	///     Is send without notification (push & sound)
	/// </summary>
	public virtual bool DisableNotification { get; set; } = false;
	
	public bool? GroupContent { get; set; } = true;
	
	public List<Guid> Attachments { get; set; }
}