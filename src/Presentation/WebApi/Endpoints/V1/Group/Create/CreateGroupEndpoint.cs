using Application.CQRS.Commands.Groups;

namespace WebApi.Endpoints.V1.Group.Create;

/// <summary>
/// Создать новую группу пользователей
/// </summary>
public sealed class CreateGroupEndpoint : Endpoint<CreateGroupRequest, int>
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public CreateGroupEndpoint(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}
	public override void Configure()
	{
		AllowAnonymous();
		Post("group");
		Version(1);
		Summary(s => {
			s.Responses[200] = "Id созданной группы";
		});
	}

	public override async Task<int> ExecuteAsync(CreateGroupRequest request, CancellationToken ct)
	{
		var command = _mapper.Map<CreateGroupCommand>(request);
		return await _mediator.Send(command, ct);
	}
}