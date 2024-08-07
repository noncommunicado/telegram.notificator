using Bll.CQRS.Commands.Groups.GroupMembersCommands;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Group.Members;

/// <summary>
///     Add members to Group
/// </summary>
public sealed class AddMembersEndpoint : Endpoint<AddOrRemoveMembersRequest>
{
	public IMapper Mapper { get; set; }
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }

	public override void Configure()
	{
		if (!AuthSettings.Value.Enabled) AllowAnonymous();
		Put("group/members");
		Version(1);
		Summary(s => s.Description = "⚠️ Either GroupId or GroupCode must be presented");
	}

	public override async Task HandleAsync(AddOrRemoveMembersRequest request, CancellationToken ct)
	{
		await Mediator.Send(Mapper.Map<AddMembersCommand>(request), ct);
	}
}