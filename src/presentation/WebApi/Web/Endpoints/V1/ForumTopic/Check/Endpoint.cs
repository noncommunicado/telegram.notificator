
using Bll.CQRS.Commands.Forum.Topic;

namespace WebApi.Web.Endpoints.V1.ForumTopic.Check;

public sealed class Endpoint : EndpointWithoutRequest
{
	public IMediator Mediator { get; set; }
	public IMapper AutoMapper { get; set; }
	public override void Configure()
	{
		AllowAnonymous();
		Get("forum/{forumId}/topic/{topicId}");
		Summary(s => {
			s.Summary = "Check forum topic exists";
			s.ResponseExamples.Add(200, new CheckForumTopicCommandResult(true,false)); 
		});
		Version(1);
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		var forumId = Route<long>("forumId");
		var topicId = Route<int>("topicId");
		CheckForumTopicCommandResult result = await Mediator.Send(new CheckForumTopicCommand(forumId,topicId), ct);
		await SendOkAsync(result, ct);
	}
}