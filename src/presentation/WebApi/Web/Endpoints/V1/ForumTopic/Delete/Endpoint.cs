using Bll.CQRS.Commands.Forum.Topic;

namespace WebApi.Web.Endpoints.V1.ForumTopic.Delete;

public sealed class Endpoint : Endpoint<Request>
{
	public IMediator Mediator { get; set; }
	public IMapper AutoMapper { get; set; }
	public override void Configure()
	{
		AllowAnonymous();
		Delete("forum/topic");
		Summary(s => {
			s.Summary = "Delete forum's topic";
			s.Responses[200] = "Topic successfully deleted";
			s.RequestExamples.Add(new RequestExample(new Request {
				ChatId = -1002158845663,
				MessageThreadId = 22
			}));
		});
		Description(x => x.Accepts<Request>("application/json"));
		Version(1);
	}

	public override async Task HandleAsync(Request request, CancellationToken ct)
	{
		var command = AutoMapper.Map<DeleteForumTopicCommand>(request);
		await Mediator.Send(command, ct);
		await SendOkAsync(ct);
	}
}