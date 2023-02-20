
using FluentValidation;

namespace WebApi.Http.Requests;

public sealed class UpdateGroupRequest : CreateGroupRequest
{
	public int Id { get; set; }
}

public sealed class UpdateGroupRequestValidator : AbstractValidator<UpdateGroupRequest>
{
	public UpdateGroupRequestValidator()
	{
		RuleFor(x => x.Id).Must(x => x > 0);
		RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
		RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(24);
		RuleFor(x => x.Chats).NotEmpty().NotNull();
	}
}