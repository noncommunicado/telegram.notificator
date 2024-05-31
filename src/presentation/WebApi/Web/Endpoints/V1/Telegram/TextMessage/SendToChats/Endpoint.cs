using Bll.CQRS.Commands.TelegramMessaging;
using Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Web.Endpoints.V1.Telegram.TextMessage.SendToChats;

/// <summary>
///     Отправить сообщение конкретным чатам
/// </summary>
[SwaggerResponse(StatusCodes.Status200OK, "Поставлено в очередь на отправку")]
public sealed class Endpoint : Endpoint<SendTextToChatsRequest>
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
		Post("telegram/send/text/to-chats");
		Summary(s => {
			s.Summary = "Send simple text message to chat or group by it number Id (long)";
			s.Description = "Text can be HTML-formatted with <a href=\"https://core.telegram.org/api/entities#allowed-entities\" target=\"_blank\">Telegram HTML rules</a><br/>" +
			                "Simple text messages allowes to be 4096 chars length only.";

			s.ExampleRequest = new SendTextToChatsRequest {
				Message = new SendMessageRequest {
					DisableNotification = false,
					GroupContent = true, Text = "Hello, Dolly! <pre>This text with telgram HTML parse mode!</pre>",
					AttachmentsIds = new List<Guid> {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()}
				},
				Chats = new []{new EnqueueChatsDto(24694612),new EnqueueChatsDto(-1237649141, 3)}
			};
		});
		Version(1);
	}

	public override async Task HandleAsync(SendTextToChatsRequest request, CancellationToken ct)
	{
		var command = _mapper.Map<EnqueueChatsMessageCommand>(request);
		command.Message.AttachmentsIds = request.Message.AttachmentsIds;
		await _mediator.Send(command, ct);
		await SendOkAsync(ct);
	}
}