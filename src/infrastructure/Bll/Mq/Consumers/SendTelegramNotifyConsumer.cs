using Bll.CQRS.Commands.TelegramMessaging;
using Domain.MqModels;
using MassTransit;

namespace Bll.Mq.Consumers;

public sealed class SendTelegramNotifyConsumer : IConsumer<SendTelegramNotifyMqMessage>
{
	private readonly MediatR.IMediator _mediator;

	public SendTelegramNotifyConsumer(MediatR.IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task Consume(ConsumeContext<SendTelegramNotifyMqMessage> context)
	{
		await _mediator.Send(new SendMessageCommand(context.Message.ChatId, context.Message.MessageId))
			.ConfigureAwait(false);
	}
}
