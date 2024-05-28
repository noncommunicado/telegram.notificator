using Bll.CQRS.Commands.TelegramMessaging;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Web.Endpoints.V1.Telegram.TextMessage.SendToGroups;

/// <summary>
///     Отправить сообщение всем чатам в группах, если есть пересечение - отправится всего одно сообщение
/// </summary>
[SwaggerResponse(StatusCodes.Status200OK, "Поставлено в очередь на отправку")]
public sealed class Endpoint : Endpoint<SendTextToGroupsRequest>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public Endpoint(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public override void Configure()
	{
		AllowAnonymous();
		Post("telegram/send/text/to-groups");
		Summary(s => {
			s.Summary = "Send simple text message to pre-made group of chats by Id (GUID)";
			s.Description = "Text can be HTML-formatted with <a href=\"https://core.telegram.org/api/entities#allowed-entities\" target=\"_blank\">Telegram HTML rules</a><br/>" +
			                "Simple text messages allowes to be 4096 chars length only.";
		});
		Version(1);
	}

	public override async Task HandleAsync(SendTextToGroupsRequest request, CancellationToken ct)
	{
		var command = _mapper.Map<EnqueueGroupsMessageCommand>(request);
		await _mediator.Send(command, ct);
		await SendOkAsync(ct);
	}
}