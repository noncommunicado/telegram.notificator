using Domain.Interfaces;
using MediatR;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bll.CQRS.Commands.Forum.Topic;
public sealed record CreateForumTopicCommand(
	long ChatId,
	string Name,
	int? IconColor = default,
	string? IconCustomEmojiId = default)
	: IRequest<ForumTopic>;
public sealed class CreateForumTopicCommandHandler : IRequestHandler<CreateForumTopicCommand, ForumTopic>
{
	private readonly IBotHostedService _botService;
	public CreateForumTopicCommandHandler(IBotHostedService botService)
	{
		_botService = botService;
	}

	public async Task<ForumTopic> Handle(CreateForumTopicCommand request, CancellationToken cancellationToken)
	{
		Log.Information("Creating forum's thread. Forum {ForumId}, thread name {ThreadName}", request.ChatId, request.Name);
		return await _botService.BotClient.CreateForumTopicAsync(request.ChatId, request.Name, request.IconColor,
			request.IconCustomEmojiId, cancellationToken);
	}
}