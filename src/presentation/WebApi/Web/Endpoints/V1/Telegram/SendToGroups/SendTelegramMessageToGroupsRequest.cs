using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Telegram.SendToGroups;

public sealed class SendTelegramMessageToGroupsRequest : SendTelegramMessageRequestBase
{
	public SendTelegramMessageToGroupsRequest() { }

	public SendTelegramMessageToGroupsRequest(string text, bool isDisableNotification, IEnumerable<Guid> groupsIds)
		: base(text, isDisableNotification)
	{
		GroupIds = groupsIds;
	}

	public IEnumerable<Guid>? GroupIds { get; set; }
	public IEnumerable<string>? GroupCodes { get; set; }
}

public class SendToGroupsRequestValidator : SendRequestBaseValidator<SendTelegramMessageToGroupsRequest>
{
	public SendToGroupsRequestValidator()
	{
		RuleFor(x => x.GroupIds)
			.NotEmpty().When(x => x.GroupCodes == null || x.GroupCodes.Any() == false)
			.WithMessage("Как минимум одна группа должна быть указана");
		RuleFor(x => x.GroupCodes)
			.NotEmpty().When(x => x.GroupIds == null || x.GroupIds.Any() == false)
			.WithMessage("Как минимум одна группа должна быть указана");
	}
}