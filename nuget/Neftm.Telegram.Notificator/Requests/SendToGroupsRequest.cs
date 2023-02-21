namespace Neftm.Telegram.Notificator.Requests;

public sealed class SendToGroupsRequest : SendRequestBase
{
	public IEnumerable<int>? GroupIds { get; set; }
	public IEnumerable<string>? GroupCodes { get; set; }

	public SendToGroupsRequest() { }

	public SendToGroupsRequest(
		string text,
		bool isDisableNotification = false,
		IEnumerable<string>? groupCodes = null, 
		IEnumerable<int>? groupIds = null) : base(text, isDisableNotification)
	{
		GroupIds = groupIds;
		GroupCodes = groupCodes;
	}
}