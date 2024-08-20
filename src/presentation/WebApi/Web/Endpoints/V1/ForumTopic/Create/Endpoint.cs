using Bll.CQRS.Commands.Forum.Topic;

namespace WebApi.Web.Endpoints.V1.ForumTopic.Create;

public sealed class Endpoint : Endpoint<Request, global::Telegram.Bot.Types.ForumTopic>
{
	public IMediator Mediator { get; set; }
	public IMapper AutoMapper { get; set; }
	public override void Configure()
	{
		AllowAnonymous();
		Post("forum/topic");
		Summary(s => {
			s.Summary = "Create forum's topic";
			s.Responses[200] = "Topic successfully created";
			s.ResponseExamples.Add(200, new global::Telegram.Bot.Types.ForumTopic() {
				MessageThreadId = 22,
				IconCustomEmojiId = "5330507314342405926",
				Name = "АЗС 060",
				IconColor = 7322096
			});
			s.RequestExamples.Add(new RequestExample(new Request {
				ChatId = -1002158845663,
				IconCustomEmojiId = "5330507314342405926",
				Name = "АЗС 060",
				IconColor = 7322096
			}));
		});
		Version(1);
	}

	public override async Task HandleAsync(Request request, CancellationToken ct)
	{
		var command = AutoMapper.Map<CreateForumTopicCommand>(request);
		var response = await Mediator.Send(command, ct);
		await SendOkAsync(response, ct);
	}
}