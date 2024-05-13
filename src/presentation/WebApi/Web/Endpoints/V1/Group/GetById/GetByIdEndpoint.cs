using Bll.CQRS.Queries.Groups;
using Domain.Dto;

namespace WebApi.Web.Endpoints.V1.Group.GetById;

/// <summary>
///     Данные группы с ее участниками
/// </summary>
public sealed class GetByIdEndpoint : Endpoint<GetGroupByIdRequest, GroupExtendedDto>
{
	private readonly IMediator _mediator;

	public GetByIdEndpoint(IMediator mediator)
	{
		_mediator = mediator;
	}

	public override void Configure()
	{
		AllowAnonymous();
		Get("group/by-id/{GroupId}");
		Summary(s => {
			s.Summary = "Группа рассылки по UID";
		});
		Version(1);
	}

	public override async Task<GroupExtendedDto> ExecuteAsync(GetGroupByIdRequest request, CancellationToken ct)
	{
		return await _mediator.Send(new GetGroupByIdQuery(request.GroupId), ct);
	}
}