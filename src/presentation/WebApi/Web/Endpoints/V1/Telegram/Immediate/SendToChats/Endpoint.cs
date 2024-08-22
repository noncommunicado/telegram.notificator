using Bll.CQRS.Commands.TelegramMessaging.Immediate;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Telegram.Immediate.SendToChats;

public sealed class Endpoint : Endpoint<SendMessageImmediatelyCommand, Response>
{
	public IMapper Mapper { get; set; }
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }
	public override void Configure()
	{
		// if (!AuthSettings.Value.Enabled) AllowAnonymous();
		AllowAnonymous();
		Post("telegram/send/text/to-chat/immediate");
		Summary(s => {
			s.Summary = "IMMEDIATE Send text message to chat";
			s.Description = "Text can be HTML-formatted with <a href=\"https://core.telegram.org/api/entities#allowed-entities\" target=\"_blank\">Telegram HTML rules</a><br/>" +
			                "Simple text messages allowes to be 4096 chars length only.";

			s.ExampleRequest = new SendMessageImmediatelyCommand(24694612,  "Hello, Dolly! <pre>This text with telgram HTML parse mode!</pre>", false);
		});
		Version(1);
	}

	public override async Task<Response> ExecuteAsync(SendMessageImmediatelyCommand req, CancellationToken ct)
	{
		return Mapper.Map<Response>(await Mediator.Send(req));
	}
}