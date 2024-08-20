
using Bll.CQRS.Commands.Forum.Topic;

namespace WebApi.Web.Endpoints.V1.ForumTopic.Create;

public sealed class Request : IMapTo<CreateForumTopicCommand>
{
	public long ChatId {get;set;}
	
	/// <summary>
	/// Topic (thread) name, 1-128 characters
	/// </summary>
	public string Name {get;set;}
	
	/// <summary>
	/// Color of the topic icon in RGB format. Currently, must be one of 7322096 (0x6FB9F0), 16766590 (0xFFD67E), 13338331 (0xCB86DB), 9367192 (0x8EEE98), 16749490 (0xFF93B2), or 16478047 (0xFB6F5F)
	/// </summary>
	public int? IconColor {get;set;}
	
	/// <summary>
	/// Unique identifier of the custom emoji shown as the topic icon
	/// </summary>
	public string? IconCustomEmojiId {get;set;}
}