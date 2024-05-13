using Bll.CQRS.Queries.Groups;
using Domain.Dto;

namespace WebApi.Web.Endpoints.V1.Group.GetAll;

/// <summary>
/// Все группы
/// </summary>
public sealed class GetAllGroupsEndpoint : Endpoint<GetAllGroupsRequest,IEnumerable<GroupDto>>
{
	private readonly IMediator _mediator;
	public GetAllGroupsEndpoint(IMediator mediator)
	{
		_mediator = mediator;
	}

	public override void Configure()
	{
		AllowAnonymous();
		Get("group/all");
		Version(1);
	}

	public override async Task<IEnumerable<GroupDto>> ExecuteAsync(GetAllGroupsRequest req, CancellationToken ct)
	{
		return await _mediator.Send(new GetAllGroupsQuery(req.ByChatIdFilter), ct);
	}
}