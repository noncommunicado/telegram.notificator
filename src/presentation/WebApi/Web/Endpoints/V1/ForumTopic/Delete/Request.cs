
using Bll.CQRS.Commands.Forum.Topic;

namespace WebApi.Web.Endpoints.V1.ForumTopic.Delete;

public sealed class Request : IMapTo<DeleteForumTopicCommand>
{
	public long ChatId {get;set;}

	public int MessageThreadId { get; set; }
}