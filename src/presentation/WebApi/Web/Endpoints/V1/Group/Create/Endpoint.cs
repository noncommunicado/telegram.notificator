using Bll.CQRS.Commands.Groups;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Web.Endpoints.V1.Group.Create;

/// <summary>
///     Создать новую группу пользователей
/// </summary>
public sealed class Endpoint : Endpoint<CreateGroupRequest, Guid>
{
	public IMapper Mapper { get; set; }
	public IMediator Mediator { get; set; }
	public IOptions<AuthorizationSettings> AuthSettings { get; set; }

	public override void Configure()
	{
		if (!AuthSettings.Value.Enabled) AllowAnonymous();
		Post("group");
		Version(1);
		Summary(s => { s.Responses[200] = "Id созданной группы"; });
		Validator<CreateGroupRequestValidator>();
	}

	public override async Task<Guid> ExecuteAsync(CreateGroupRequest request, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		var command = Mapper.Map<CreateGroupCommand>(request);
		return await Mediator.Send(command, ct);
	}
}