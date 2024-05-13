using FluentValidation;
using WebApi.Web.Endpoints.V1.Group.Create;

namespace WebApi.Web.Endpoints.V1.Group.Update;

public sealed class UpdateGroupRequest : CreateGroupRequest
{
	public Guid Id { get; set; }
}

public sealed class UpdateGroupRequestValidator : AbstractValidator<UpdateGroupRequest>
{
	public UpdateGroupRequestValidator()
	{
		RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
		RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(100);
	}
}