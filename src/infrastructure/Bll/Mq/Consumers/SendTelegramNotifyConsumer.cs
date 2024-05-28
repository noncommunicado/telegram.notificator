using Bll.CQRS.Commands.TelegramMessaging;
using MassTransit;

namespace Bll.Mq.Consumers;

public sealed class SendTelegramNotifyMqMessage
{
	public SendTelegramNotifyMqMessage() { }

	public SendTelegramNotifyMqMessage(long chatId, int threadId, Guid messageId)
	{
		ChatId = chatId;
		MessageId = messageId;
		ThreadId = threadId;
	}

	public long ChatId { get; set; }
	public int ThreadId { get; set; }
	public Guid MessageId { get; set; }
}

public sealed class SendTelegramNotifyConsumer : IConsumer<SendTelegramNotifyMqMessage>
{
	private readonly MediatR.IMediator _mediator;
	public SendTelegramNotifyConsumer(MediatR.IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task Consume(ConsumeContext<SendTelegramNotifyMqMessage> context)
	{
		await _mediator.Send(
				new SendMessageCommand(context.Message.ChatId, context.Message.ThreadId, context.Message.MessageId))
			.ConfigureAwait(false);
	}
}
