using FluentValidation;

namespace WebApi.Http.Requests;

public sealed class SendToGroupsRequest : SendRequestBase
{
	public SendToGroupsRequest() { }

	public SendToGroupsRequest(string text, bool isDisableNotification, IEnumerable<int> groupsIds) 
		: base(text, isDisableNotification) 
	{
		GroupIds = groupsIds;
	}

	public IEnumerable<int>? GroupIds { get; set; }
	public IEnumerable<string>? GroupCodes { get; set; }
}

public class SendToGroupsRequestValidator : SendRequestBaseValidator<SendToGroupsRequest>
{
	public SendToGroupsRequestValidator() {
		RuleFor(x => x.GroupIds)
			.NotEmpty().When(x =>  x.GroupCodes== null || x.GroupCodes.Any() == false)
			.WithMessage("Как минимум одна группа должна быть указана");
		RuleFor(x => x.GroupCodes)
			.NotEmpty().When(x => x.GroupIds == null || x.GroupIds.Any() == false)
			.WithMessage("Как минимум одна группа должна быть указана");
	}
}