using Bll.CQRS.Commands.TelegramMessaging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Telegram.TextMessage.SendToGroups;

/// <summary>
///     Отправить сообщение всем чатам в группах, если есть пересечение - отправится всего одно сообщение
/// </summary>
[SwaggerResponse(StatusCodes.Status200OK, "Поставлено в очередь на отправку")]
public sealed class Endpoint : Endpoint<SendTextToGroupsRequest>
{
	public IMapper Mapper { get; set; }
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }
	public override void Configure()
	{
		//if (!AuthSettings.Value.Enabled) AllowAnonymous();
		AllowAnonymous();
		Post("telegram/send/text/to-groups");
		Summary(s => {
			s.Summary = "Send simple text message to pre-made group of chats by Id (GUID)";
			s.Description = "Text can be HTML-formatted with <a href=\"https://core.telegram.org/api/entities#allowed-entities\" target=\"_blank\">Telegram HTML rules</a><br/>" +
			                "Simple text messages allowes to be 4096 chars length only.<br/><br/>" +
			                "You can send GroupIds or/and GroupCodes, messages will be send distinct!";
			s.ExampleRequest = new SendTextToGroupsRequest {
				Message = new SendMessageRequest {
					DisableNotification = false,
					GroupContent = true, Text = "Hello, Dolly! <pre>This text with telgram HTML parse mode!</pre>",
					AttachmentsIds = new List<Guid> {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()}
				},
				GroupCodes = new []{ "test_group_1", "some_mailing_group_2" },
				GroupIds = new []{Guid.NewGuid(), Guid.NewGuid()}
			};
		});
		Version(1);
	}

	public override async Task HandleAsync(SendTextToGroupsRequest request, CancellationToken ct)
	{
		var command = Mapper.Map<EnqueueGroupsMessageCommand>(request);
		command.Message.AttachmentsIds = request.Message.AttachmentsIds;
		await Mediator.Send(command, ct);
		await SendOkAsync(ct);
	}
}