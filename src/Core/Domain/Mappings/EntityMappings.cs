
using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using Domain.Models;

namespace Domain.Mappings;

public sealed class EntityMappings : Profile
{
	public EntityMappings()
	{
		CreateMap<MessageModel, MessageEntity>().ReverseMap();
		CreateMap<GroupEntity, GroupDto>();
		CreateMap<GroupEntity, GroupExtendedDto>()
			.ForMember(x => x.Chats, opt =>
				opt.MapFrom(z => (z.Members ?? Array.Empty<GroupMemberEntity>()).Select(m => m.ChatId)))
			.ForMember(x => x.Code, opt => 
				opt.MapFrom(z => z.SysCode));
	}
}