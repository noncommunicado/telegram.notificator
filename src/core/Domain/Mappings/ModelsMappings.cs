using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Domain.Mappings;

public sealed class ModelsMappings : Profile
{
	public ModelsMappings()
	{
		CreateMap<MessageBase, SendCreatedMessageRequest>().ReverseMap();
		CreateMap<MessageBase, SendMessageRequest>().ReverseMap();
		CreateMap<MessageEntity, CachedSendMessageRequest>().ReverseMap();
		CreateMap<AttachmentEntity, CachedSendMessageAttachment>().ReverseMap();
		
		CreateMap<MessageEntity, SendCreatedMessageRequest>().ReverseMap();
		CreateMap<MessageEntity, SendMessageRequest>().ReverseMap();
	}
}