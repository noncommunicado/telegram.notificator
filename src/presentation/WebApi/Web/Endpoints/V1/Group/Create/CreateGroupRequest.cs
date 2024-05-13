using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Group.Create;

public class CreateGroupRequest
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
		RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(100);
	}
}