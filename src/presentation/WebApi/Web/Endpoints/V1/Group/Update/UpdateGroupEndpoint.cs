using Bll.CQRS.Commands.Groups;

namespace WebApi.Web.Endpoints.V1.Group.Update;

/// <summary>
///     Обновить группу пользователей, в том числе самих пользователей
/// </summary>
/// <remarks>
///     Полностью обновляет пользователей, удаляет невключенных в новый список и создает новых
/// </remarks>
public sealed class UpdateGroupEndpoint : Endpoint<UpdateGroupRequest>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public UpdateGroupEndpoint(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}

	public override void Configure()
	{
		AllowAnonymous();
		Put("group");
		Version(1);
	}

	public override async Task HandleAsync(UpdateGroupRequest request, CancellationToken ct)
	{
		var command = _mapper.Map<UpdateGroupCommand>(request);
		await _mediator.Send(command, ct);
	}
}