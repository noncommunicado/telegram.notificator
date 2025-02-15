using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using Domain.Models;

namespace Domain.Mappings;

public sealed class EntityMappings : Profile
{
	public EntityMappings()
	{
		CreateMap<GroupEntity, GroupDto>()
			.ForMember(x => x.Code, opt =>
				opt.MapFrom(z => z.SysCode))
			.ForMember(x => x.MembersCount, opt => 
				opt.MapFrom(z => z.Members.Count()));

		CreateMap<GroupEntity, GroupExtendedDto>()
			.ForMember(x => x.Code, opt =>
				opt.MapFrom(z => z.SysCode));

		CreateMap<GroupMemberEntity, GroupMemberEditCreateDto>().ReverseMap();
		CreateMap<GroupMemberEntity, GroupMemberDto>().ReverseMap();
		CreateMap<GroupMemberEditCreateDto, GroupMemberDto>().ReverseMap();
		CreateMap<AttachmentEntity, AttachmentDto>().ReverseMap();
	}
}