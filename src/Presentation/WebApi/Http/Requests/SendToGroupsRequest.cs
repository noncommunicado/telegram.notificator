
using Domain.Models;
using FluentValidation;

namespace WebApi.Http.Requests;

public sealed class SendToGroupsRequest : SendRequestBase
{
	public List<int> Groups { get; set; }
}

public class SendToGroupsRequestValidator : SendRequestBaseValidator<SendToGroupsRequest>
{
	public SendToGroupsRequestValidator() : base()
	{
		RuleFor(x => x.Groups)
			.NotEmpty().WithMessage("Как минимум одна группа должна быть указана");
	}
}