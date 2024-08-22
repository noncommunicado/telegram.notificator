
using Telegram.Bot.Types;

namespace WebApi.Web.Endpoints.V1.Telegram.Immediate.SendToChats;

public sealed class Response : IMapFrom<Message>
{
	/// <summary>Unique message identifier inside this chat</summary>
	public int MessageId { get; set; }

	/// <summary><em>Optional</em>. Unique identifier of a message thread to which the message belongs; for supergroups only</summary>
	public int? MessageThreadId { get; set; }
}