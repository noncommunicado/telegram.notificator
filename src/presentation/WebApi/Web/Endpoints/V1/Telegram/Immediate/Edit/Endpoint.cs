using Bll.CQRS.Commands.TelegramMessaging.Immediate;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Telegram.Immediate.Edit;

public sealed class Endpoint : Endpoint<EditMessageImmediatelyCommand, Response>
{
	public IMapper Mapper { get; set; }
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }
	public override void Configure()
	{
		// if (!AuthSettings.Value.Enabled) AllowAnonymous();
		AllowAnonymous();
		Put("telegram/edit/text/to-chat/immediate");
		Summary(s => {
			s.Summary = "IMMEDIATE Edit text message to chat";
			s.Description = "Text can be HTML-formatted with <a href=\"https://core.telegram.org/api/entities#allowed-entities\" target=\"_blank\">Telegram HTML rules</a><br/>" +
			                "Simple text messages allowes to be 4096 chars length only.";

			s.ExampleRequest = new EditMessageImmediatelyCommand(24694612, 21,  "Hello, Dolly! <pre>This text with telgram HTML parse mode!</pre>");
		});
		Version(1);
	}

	public override async Task<Response> ExecuteAsync(EditMessageImmediatelyCommand req, CancellationToken ct)
	{
		return Mapper.Map<Response>(await Mediator.Send(req));
	}
}