using Application.CQRS.Commands.TelegramMessaging;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Endpoints.V1.Telegram.SendToChats;

/// <summary>
/// Отправить сообщение конкретным чатам
/// </summary>
[SwaggerResponse(StatusCodes.Status200OK, description: "Поставлено в очередь на отправку")]
public sealed class SendToChatsEndpoint : Endpoint<SendTelegramMessageToChatsRequest>
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;
	public SendToChatsEndpoint(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	
	public override void Configure()
	{
		AllowAnonymous();
		Post("telegram/send/chats");
		Version(1);
	}

	public override async Task HandleAsync(SendTelegramMessageToChatsRequest request, CancellationToken ct)
	{
		var command = _mapper.Map<EnqueueChatsMessageCommand>(request);
		await _mediator.Send(command, ct);
		await SendOkAsync(ct);
	}
}