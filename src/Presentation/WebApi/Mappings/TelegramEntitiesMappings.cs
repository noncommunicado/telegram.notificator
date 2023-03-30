
using Application.CQRS.Commands.Groups;
using AutoMapper;
using WebApi.Endpoints.V1.Group.Create;
using WebApi.Endpoints.V1.Group.Update;

namespace WebApi.Mappings;

public sealed class TelegramEntitiesMappings : Profile
{
	public TelegramEntitiesMappings()
	{
		CreateMap<CreateGroupRequest, CreateGroupCommand>();
		CreateMap<UpdateGroupRequest, UpdateGroupCommand>();
	}
}