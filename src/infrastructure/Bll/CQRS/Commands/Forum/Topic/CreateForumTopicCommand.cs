using Domain.Interfaces;
using MediatR;
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

	public Task<ForumTopic> Handle(CreateForumTopicCommand request, CancellationToken cancellationToken)
	{
		return _botService.BotClient.CreateForumTopicAsync(request.ChatId, request.Name, request.IconColor,
			request.IconCustomEmojiId, cancellationToken);
	}
}