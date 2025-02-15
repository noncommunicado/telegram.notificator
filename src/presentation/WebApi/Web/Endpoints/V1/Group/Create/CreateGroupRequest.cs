using Domain.Dto;
using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Group.Create;

public class CreateGroupRequest
{
	public string Name { get; set; }
	public string Code { get; set; }
	public IEnumerable<GroupMemberEditCreateDto>? Members { get; set; }
}

public class CreateGroupRequestValidator : Validator<CreateGroupRequest>
{
	public CreateGroupRequestValidator()
	{
		RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
		RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(100);
	}
}