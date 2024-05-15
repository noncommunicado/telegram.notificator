using Domain.Models;
using Domain.MqModels;
using MassTransit;
using MediatR;

namespace Bll.CQRS.Commands.TelegramMessaging;

public sealed record EnqueueChatsDto(long ChatId, int ThreadId = -1);
public sealed record EnqueueChatsMessageCommand(IEnumerable<EnqueueChatsDto> Chats, MessageModel Message) : IRequest;

public sealed class EnqueueChatsMessageCommandHandler : IRequestHandler<EnqueueChatsMessageCommand>
{
	private readonly IMediator _mediator;
	private readonly IPublishEndpoint _pEnd;

	public EnqueueChatsMessageCommandHandler(
		IPublishEndpoint pEnd, IMediator mediator)
	{
		_pEnd = pEnd;
		_mediator = mediator;
	}

	public async Task Handle(EnqueueChatsMessageCommand request, CancellationToken ct)
	{
		var messageId = await _mediator.Send(new CreateMessageCommand(request.Message), ct);
		foreach (var chat in request.Chats)
			await _pEnd.Publish(new SendTelegramNotifyMqMessage(chat.ChatId, chat.ThreadId, messageId), ct);
	}
}