using Bll.CQRS.Commands.Groups.GroupMembersCommands;
using Bll.CQRS.Commands.TelegramMessaging;
using WebApi.Web.Endpoints.V1.Group.Members;
using WebApi.Web.Endpoints.V1.Telegram.TextMessage.SendToChats;
using WebApi.Web.Endpoints.V1.Telegram.TextMessage.SendToGroups;

namespace WebApi.Mappings;

public sealed class SendCommandsMappings : Profile
{
	public SendCommandsMappings()
	{
		CreateMap<SendTextToChatsRequest, EnqueueChatsMessageCommand>();
		CreateMap<SendTextToGroupsRequest, EnqueueGroupsMessageCommand>();

		CreateMap<AddOrRemoveMembersRequest, AddMembersCommand>();
		CreateMap<AddOrRemoveMembersRequest, RemoveMembersCommand>();
	}
}