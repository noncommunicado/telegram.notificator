using Bll.CQRS.Commands.Groups.GroupMembersCommands;

namespace WebApi.Web.Endpoints.V1.Group.Members;

/// <summary>
/// Add members to Group
/// </summary>
public sealed class AddMembersEndpoint : Endpoint<AddOrRemoveMembersRequest>
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;
	public AddMembersEndpoint(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}
	
	public override void Configure()
	{
		AllowAnonymous();
		Put("group/members");
		Version(1);
	}

	public override async Task HandleAsync(AddOrRemoveMembersRequest request, CancellationToken ct)
	{
		await _mediator.Send(_mapper.Map<AddMembersCommand>(request), ct);
	}
}