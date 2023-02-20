using AutoMapper;
using Domain.Models;

namespace Domain.Mappings;

public sealed class ModelsMappings : Profile
{
	public ModelsMappings()
	{
		CreateMap<MessageBase, MessageModel>().ReverseMap();
	}
}