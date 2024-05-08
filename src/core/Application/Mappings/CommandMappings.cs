
using Application.CQRS.Commands;
using Application.CQRS.Commands.TelegramMessaging;
using AutoMapper;
using Domain.MqModels;

namespace Application.Mappings;

public sealed class CommandMappings : Profile
{
	public CommandMappings()
	{
		CreateMap<SendTelegramNotifyMqMessage, SendMessageCommand>();
	}
}