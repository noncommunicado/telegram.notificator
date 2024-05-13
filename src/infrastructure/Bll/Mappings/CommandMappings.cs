using AutoMapper;
using Bll.CQRS.Commands.TelegramMessaging;
using Domain.MqModels;

namespace Bll.Mappings;

public sealed class CommandMappings : Profile
{
	public CommandMappings()
	{
		CreateMap<SendTelegramNotifyMqMessage, SendMessageCommand>();
	}
}