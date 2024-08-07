using Bll.CQRS.Queries.Groups;
using Domain.Dto;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Group.GetById;

/// <summary>
///     Данные группы с ее участниками
/// </summary>
public sealed class Endpoint : Endpoint<GetGroupByIdRequest, GroupExtendedDto>
{
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }

	public override void Configure()
	{
		if (!AuthSettings.Value.Enabled) AllowAnonymous();
		Get("group/by-id/{GroupId}");
		Summary(s => {
			s.Summary = "Группа рассылки по UID";
		});
		Version(1);
	}

	public override async Task<GroupExtendedDto> ExecuteAsync(GetGroupByIdRequest request, CancellationToken ct)
	{
		return await Mediator.Send(new GetGroupByIdQuery(request.GroupId), ct);
	}
}