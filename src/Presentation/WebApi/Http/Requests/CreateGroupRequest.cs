
using FluentValidation;

namespace WebApi.Http.Requests;

public  class CreateGroupRequest
{
	public string Name { get; set; }
	public string Code { get; set; }
	public IEnumerable<long>? Chats { get; set; }
}

public sealed class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>
{
	public CreateGroupRequestValidator()
	{
		RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
		RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(24);
	}
}