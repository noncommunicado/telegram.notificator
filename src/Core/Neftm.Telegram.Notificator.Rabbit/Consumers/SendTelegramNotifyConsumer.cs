using Application.CQRS.Commands.TelegramMessaging;
using AutoMapper;
using Domain.MqModels;
using MassTransit;
using MediatR;

namespace Neftm.Telegram.Notificator.Rabbit.Consumers;

public sealed class SendTelegramNotifyConsumer : IConsumer<SendTelegramNotifyMqMessage>
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;
	public SendTelegramNotifyConsumer(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}
	public async Task Consume(ConsumeContext<SendTelegramNotifyMqMessage> context)
	{
		var command = _mapper.Map<SendMessageCommand>(context.Message);
		await _mediator.Send(command).ConfigureAwait(false);
	}
}