using Bll.CQRS.Queries.Groups;
using Domain.Dto;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Group.GetByCode;

public sealed class Endpoint : Endpoint<GetGroupByCodeRequest, GroupExtendedDto>
{
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }

	public override void Configure()
	{
		if (!AuthSettings.Value.Enabled) AllowAnonymous();
		Get("group/by-code/{GroupCode}");
		Summary(s => {
			s.Summary = "Группа рассылки по ее уникальному коду";
		});
		Version(1);
	}

	public override async Task<GroupExtendedDto> ExecuteAsync(GetGroupByCodeRequest request, CancellationToken ct)
	{
		return await Mediator.Send(new GetGroupByCodeQuery(request.GroupCode), ct);
	}
}