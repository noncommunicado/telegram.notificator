using Application.CQRS.Commands.TelegramMessaging;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Web.Endpoints.V1.Telegram.SendToGroups;

/// <summary>
/// Отправить сообщение всем чатам в группах, если есть пересечение - отправится всего одно сообщение
/// </summary>
[SwaggerResponse(StatusCodes.Status200OK, description: "Поставлено в очередь на отправку")]
public sealed class SendToGroupsEndpoint : Endpoint<SendTelegramMessageToGroupsRequest>
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;
	public SendToGroupsEndpoint(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	public override void Configure()
	{
		AllowAnonymous();
		Post("telegram/send/groups");
		Version(1);
	}

	public override async Task HandleAsync(SendTelegramMessageToGroupsRequest request, CancellationToken ct)
	{
		var command = _mapper.Map<EnqueueGroupsMessageCommand>(request);
		await _mediator.Send(command, ct);
		await SendOkAsync(ct);
	}
}