using Application.CQRS.Commands;
using Application.CQRS.Commands.Groups.GroupMembersCommands;
using Application.CQRS.Commands.TelegramMessaging;
using AutoMapper;
using WebApi.Web.Endpoints.V1.Group.Members;
using WebApi.Web.Endpoints.V1.Telegram.SendToChats;
using WebApi.Web.Endpoints.V1.Telegram.SendToGroups;

namespace WebApi.Mappings;

public sealed class SendCommandsMappings : Profile
{
	public SendCommandsMappings()
	{
		CreateMap<SendTelegramMessageToChatsRequest, EnqueueChatsMessageCommand>();
		CreateMap<SendTelegramMessageToGroupsRequest, EnqueueGroupsMessageCommand>();

		CreateMap<AddOrRemoveMembersRequest, AddMembersCommand>();
		CreateMap<AddOrRemoveMembersRequest, RemoveMembersCommand>();
	}
}