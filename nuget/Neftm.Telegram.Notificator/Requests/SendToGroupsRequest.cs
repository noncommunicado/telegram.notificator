namespace Neftm.Telegram.Notificator.Requests;

public sealed class SendToGroupsRequest : SendRequestBase
{
	public SendToGroupsRequest() { }

	public SendToGroupsRequest(
		string text,
		bool isDisableNotification = false,
		IEnumerable<string>? groupCodes = null,
		IEnumerable<Guid>? groupIds = null) : base(text, isDisableNotification)
	{
		GroupIds = groupIds;
		GroupCodes = groupCodes;
	}

	public IEnumerable<Guid>? GroupIds { get; set; }
	public IEnumerable<string>? GroupCodes { get; set; }
}