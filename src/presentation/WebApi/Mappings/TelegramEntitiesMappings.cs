using Bll.CQRS.Commands.Groups;
using WebApi.Web.Endpoints.V1.Group.Create;
using WebApi.Web.Endpoints.V1.Group.Update;

namespace WebApi.Mappings;

public sealed class TelegramEntitiesMappings : Profile
{
	public TelegramEntitiesMappings()
	{
		CreateMap<CreateGroupRequest, CreateGroupCommand>();
		CreateMap<UpdateGroupRequest, UpdateGroupCommand>();
	}
}