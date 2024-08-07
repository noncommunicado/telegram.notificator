using Bll.CQRS.Commands.Groups;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Group.Delete;

/// <summary>
///     Удалить группу
/// </summary>
public sealed class Endpoint : Endpoint<DeleteGroupByIdRequest>
{
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }

	public override void Configure()
	{
		if (!AuthSettings.Value.Enabled) AllowAnonymous();
		Delete("group/{GroupId}");
		Version(1);
	}

	public override async Task HandleAsync(DeleteGroupByIdRequest req, CancellationToken ct)
	{
		await Mediator.Send(new DeleteGroupByIdCommand(req.GroupId), ct);
	}
}