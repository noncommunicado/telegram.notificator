using Bll.CQRS.Commands.Groups.GroupMembersCommands;
using Bll.CQRS.Commands.TelegramMessaging;
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