
using Application.CQRS.Commands.Groups;
using AutoMapper;
using WebApi.Http.Requests;

namespace WebApi.Mappings;

public sealed class TelegramEntitiesMappings : Profile
{
	public TelegramEntitiesMappings()
	{
		CreateMap<CreateGroupRequest, CreateGroupCommand>();
		CreateMap<UpdateGroupRequest, UpdateGroupCommand>();
	}
}