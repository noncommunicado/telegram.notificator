using Bll.CQRS.Commands.TelegramMessaging;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Web.Endpoints.V1.Telegram.SendToChats;

/// <summary>
///     Отправить сообщение конкретным чатам
/// </summary>
[SwaggerResponse(StatusCodes.Status200OK, "Поставлено в очередь на отправку")]
public sealed class SendToChatsEndpoint : Endpoint<SendTelegramMessageToChatsRequest>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

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