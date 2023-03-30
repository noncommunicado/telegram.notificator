using Application.CQRS.Queries.Groups;
using Domain.Dto;

namespace WebApi.Endpoints.V1.Group.GetById;

/// <summary>
/// Данные группы с ее участниками
/// </summary>
public sealed class GetGroupByIdEndpoint : Endpoint<GetGroupByIdRequest,GroupExtendedDto>
{
	private readonly IMediator _mediator;
	public GetGroupByIdEndpoint(IMediator mediator)
	{
		_mediator = mediator;
	}

	public override void Configure()
	{
		AllowAnonymous();
		Get("group/{GroupId}");
		Version(1);
	}

	public override async Task<GroupExtendedDto> ExecuteAsync(GetGroupByIdRequest request, CancellationToken ct)
	{
		return await _mediator.Send(new GetGroupQuery(request.GroupId), ct);
	}
}