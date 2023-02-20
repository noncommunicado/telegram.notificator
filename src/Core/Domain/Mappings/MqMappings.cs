
using AutoMapper;
using Domain.Models;
using Domain.MqModels;

namespace Domain.Mappings;

public sealed class MqMappings : Profile
{
	public MqMappings()
	{
		CreateMap<NotificationModelBase, SendTelegramNotifyMqMessage>();
	}
}