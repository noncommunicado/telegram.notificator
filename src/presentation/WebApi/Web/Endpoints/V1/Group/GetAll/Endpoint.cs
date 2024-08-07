using Bll.CQRS.Queries.Groups;
using Domain.Dto;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Group.GetAll;

/// <summary>
///     Все группы
/// </summary>
public sealed class Endpoint : Endpoint<GetAllGroupsRequest, IEnumerable<GroupDto>>
{
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }

	public override void Configure()
	{
		if (!AuthSettings.Value.Enabled) AllowAnonymous();
		Get("group/all");
		Version(1);
	}

	public override async Task<IEnumerable<GroupDto>> ExecuteAsync(GetAllGroupsRequest req, CancellationToken ct)
	{
		return await Mediator.Send(new GetAllGroupsQuery(req.ByChatIdFilter), ct);
	}
}