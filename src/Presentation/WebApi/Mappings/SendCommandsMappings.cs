using Application.CQRS.Commands;
using Application.CQRS.Commands.TelegramMessaging;
using AutoMapper;
using WebApi.Http.Requests;

namespace WebApi.Mappings;

public sealed class SendCommandsMappings : Profile
{
	public SendCommandsMappings()
	{
		CreateMap<SendToChatsRequest, EnqueueChatsMessageCommand>();
		CreateMap<SendToGroupsRequest, EnqueueGroupsMessageCommand>();
	}
}