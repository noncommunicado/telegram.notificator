
using Bll.CQRS.Commands.TelegramMessaging;
using Domain.MqModels;
using MassTransit;
using MediatR;

namespace Neftm.Telegram.Notificator.Rabbit.Consumers;

public sealed class SendTelegramNotifyConsumer : IConsumer<SendTelegramNotifyMqMessage>
{
	private readonly IMediator _mediator;
	public SendTelegramNotifyConsumer(IMediator mediator)
	{
		_mediator = mediator;
	}
	public async Task Consume(ConsumeContext<SendTelegramNotifyMqMessage> context)
	{
		await _mediator.Send(new SendMessageCommand(context.Message.ChatId, context.Message.MessageId))
			.ConfigureAwait(false);
	}
}