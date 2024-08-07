using Bll.CQRS.Commands.Groups;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Group.Update;

/// <summary>
///     Обновить группу пользователей, в том числе самих пользователей
/// </summary>
/// <remarks>
///     Полностью обновляет пользователей, удаляет невключенных в новый список и создает новых
/// </remarks>
public sealed class Endpoint : Endpoint<UpdateGroupRequest>
{
	public IMapper Mapper { get; set; }
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }

	public override void Configure()
	{
		if (!AuthSettings.Value.Enabled) AllowAnonymous();
		Put("group/{GroupId}");
		Version(1);
	}

	public override async Task HandleAsync(UpdateGroupRequest request, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		var command = Mapper.Map<UpdateGroupCommand>(request);
		await Mediator.Send(command, ct);
	}
}