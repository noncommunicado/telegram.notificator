using Bll.CQRS.Queries.Groups;
using Domain.Dto;

namespace WebApi.Web.Endpoints.V1.Group.GetByCode;

public sealed class Endpoint : Endpoint<GetGroupByCodeRequest, GroupExtendedDto>
{
	private readonly IMediator _mediator;
	public Endpoint(IMediator mediator) => _mediator = mediator;

	public override void Configure()
	{
		AllowAnonymous();
		Get("group/by-code/{GroupCode}");
		Summary(s => {
			s.Summary = "Группа рассылки по ее уникальному коду";
		});
		Version(1);
	}

	public override async Task<GroupExtendedDto> ExecuteAsync(GetGroupByCodeRequest request, CancellationToken ct)
	{
		return await _mediator.Send(new GetGroupByCodeQuery(request.GroupCode), ct);
	}
}