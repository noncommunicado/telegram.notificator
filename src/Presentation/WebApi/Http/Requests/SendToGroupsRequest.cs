
using Domain.Models;
using FluentValidation;

namespace WebApi.Http.Requests;

public sealed class SendToGroupsRequest : SendRequestBase
{
	public IEnumerable<int> Groups { get; set; }

	public SendToGroupsRequest()
	{
		
	}
	public SendToGroupsRequest(string text, bool isDisableNotification, IEnumerable<int> groups) : base(text, isDisableNotification)
	{
		this.Groups = groups;
	}
}

public class SendToGroupsRequestValidator : SendRequestBaseValidator<SendToGroupsRequest>
{
	public SendToGroupsRequestValidator() : base()
	{
		RuleFor(x => x.Groups)
			.NotEmpty().WithMessage("Как минимум одна группа должна быть указана");
	}
}