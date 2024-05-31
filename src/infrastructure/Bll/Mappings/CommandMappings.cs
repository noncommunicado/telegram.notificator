using AutoMapper;
using Bll.CQRS.Commands.TelegramMessaging;
using Bll.Mq.Consumers;

namespace Bll.Mappings;

public sealed class CommandMappings : Profile
{
	public CommandMappings()
	{
		CreateMap<SendTelegramNotifyMqMessage, SendMessageCommand>();
	}
}