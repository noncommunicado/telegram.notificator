using Domain.Interfaces;
using MediatR;
using Telegram.Bot;

namespace Bll.CQRS.Commands.Forum.Topic;
public sealed record DeleteForumTopicCommand(long ChatId, int MessageThreadId) : IRequest;
public sealed class DeleteForumTopicCommandHandler : IRequestHandler<DeleteForumTopicCommand>
{
	private readonly IBotHostedService _botService;
	public DeleteForumTopicCommandHandler(IBotHostedService botService)
	{
		_botService = botService;
	}

	public Task Handle(DeleteForumTopicCommand request, CancellationToken cancellationToken)
	{
		return _botService.BotClient
			.DeleteForumTopicAsync(request.ChatId, request.MessageThreadId, cancellationToken);
	}
}