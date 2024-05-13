using Bll.CQRS.Commands.Groups;

namespace WebApi.Web.Endpoints.V1.Group.Delete;

/// <summary>
/// Удалить группу
/// </summary>
public sealed class DeleteGroupEndpoint : Endpoint<DeleteGroupByIdRequest>
{
	private readonly IMediator _mediator;

	public DeleteGroupEndpoint(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	public override void Configure()
	{
		AllowAnonymous();
		Delete("group/{GroupId}");
		Version(1);
	}

	public override async Task HandleAsync(DeleteGroupByIdRequest req, CancellationToken ct)
	{
		await _mediator.Send(new DeleteGroupByIdCommand(req.GroupId), ct);
	}
}