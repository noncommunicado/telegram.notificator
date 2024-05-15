using Domain.Dto;
using FluentValidation;

namespace WebApi.Web.Endpoints.V1.Group.Members;

public sealed class AddOrRemoveMembersRequest
{
	public Guid? GroupId { get; set; }
	public string? GroupCode { get; set; }
	public IEnumerable<GroupMemberEditCreateDto> Members { get; set; }
}

public sealed class AddOrRemoveMemberRequestValidator : AbstractValidator<AddOrRemoveMembersRequest>
{
	public AddOrRemoveMemberRequestValidator()
	{
		RuleFor(x => x.Members).NotEmpty().NotNull();
		RuleFor(x => x.GroupCode)
			.NotNull().When(x => x.GroupId == null)
			.WithMessage("Нужно указать Id группы или ее код");
		RuleFor(x => x.GroupId)
			.NotNull().When(x => string.IsNullOrEmpty(x.GroupCode))
			.WithMessage("Нужно указать Id группы или ее код");
	}
}