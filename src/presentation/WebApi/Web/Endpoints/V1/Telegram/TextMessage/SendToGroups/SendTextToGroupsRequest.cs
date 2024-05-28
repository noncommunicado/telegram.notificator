using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Telegram.TextMessage.SendToGroups;

public sealed class SendTextToGroupsRequest : SendTextRequestBase
{
	public SendTextToGroupsRequest() { }

	public SendTextToGroupsRequest(string text, bool isDisableNotification, IEnumerable<Guid> groupsIds)
		: base(text, isDisableNotification)
	{
		GroupIds = groupsIds;
	}

	public IEnumerable<Guid>? GroupIds { get; set; }
	public IEnumerable<string>? GroupCodes { get; set; }
}

public class SendTextToGroupsRequestValidator : SendTextRequestBaseValidator<SendTextToGroupsRequest>
{
	public SendTextToGroupsRequestValidator()
	{
		RuleFor(x => x.GroupIds)
			.NotEmpty().When(x => x.GroupCodes == null || x.GroupCodes.Any() == false)
			.WithMessage("Как минимум одна группа должна быть указана");
		RuleFor(x => x.GroupCodes)
			.NotEmpty().When(x => x.GroupIds == null || x.GroupIds.Any() == false)
			.WithMessage("Как минимум одна группа должна быть указана");
	}
}