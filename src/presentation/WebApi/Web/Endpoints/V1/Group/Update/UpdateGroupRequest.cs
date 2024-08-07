using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Group.Update;

public sealed class UpdateGroupRequest 
{
	public Guid GroupId { get; set; }
	public string Name { get; set; }
	public string Code { get; set; }
}

public class UpdateGroupRequestValidator : Validator<UpdateGroupRequest>
{
	public UpdateGroupRequestValidator()
	{
		RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
		RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(100);
	}
}