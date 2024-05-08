using Domain.Models;
using Domain.MqModels;
using MassTransit;
using MediatR;

namespace Application.CQRS.Commands.TelegramMessaging;

public sealed record EnqueueChatsMessageCommand(IEnumerable<long> Chats, MessageModel Message) : IRequest;

public sealed class EnqueueChatsMessageCommandHandler : IRequestHandler<EnqueueChatsMessageCommand>
{
	private readonly IPublishEndpoint _pEnd;
	private readonly IMediator _mediator;
	public EnqueueChatsMessageCommandHandler(
		IPublishEndpoint pEnd, IMediator mediator)
	{
		_pEnd = pEnd;
		_mediator = mediator;
	}

	public async Task Handle(EnqueueChatsMessageCommand request, CancellationToken ct)
	{
		var messageId = await _mediator.Send(new CreateMessageCommand(request.Message), ct);
		foreach (var chatId in request.Chats)
			await _pEnd.Publish(new SendTelegramNotifyMqMessage(chatId, messageId), ct);
	}
}