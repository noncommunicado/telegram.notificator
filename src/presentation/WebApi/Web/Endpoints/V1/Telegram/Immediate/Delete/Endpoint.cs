using Bll.CQRS.Commands.TelegramMessaging.Immediate;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Telegram.Immediate.Delete;

public sealed class Endpoint : Endpoint<DeleteMessageImmediatelyCommand>
{
	public IMapper Mapper { get; set; }
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }
	public override void Configure()
	{
		// if (!AuthSettings.Value.Enabled) AllowAnonymous();
		AllowAnonymous();
		Delete("telegram/edit/text/to-chat/immediate");
		Summary(s => {
			s.Summary = "IMMEDIATE delete chat message"; 
		});
		Description(x => x.Accepts<DeleteMessageImmediatelyCommand>("application/json"));
		Version(1);
	}

	public override async Task HandleAsync(DeleteMessageImmediatelyCommand req, CancellationToken ct)
	{
		await Mediator.Send(req);
		await SendOkAsync(ct);
	}
}