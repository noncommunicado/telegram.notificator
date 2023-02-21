
using FluentValidation;

namespace WebApi.Http.Requests;

public sealed class AddOrRemoveMembersRequest
{
	public int? GroupId { get; set; }
	public string? GroupCode { get; set; }
	public IEnumerable<long> Chats { get; set; }
}

public sealed class AddOrRemoveMemberRequestValidator : AbstractValidator<AddOrRemoveMembersRequest>
{
	public AddOrRemoveMemberRequestValidator()
	{
		RuleFor(x => x.Chats).NotEmpty().NotNull();
		RuleFor(x => x.GroupCode)
			.NotNull().When(x => x.GroupId == null)
			.WithMessage("Нужно указать Id группы или ее код");
		RuleFor(x => x.GroupId)
			.NotNull().When(x => string.IsNullOrEmpty( x.GroupCode))
			.WithMessage("Нужно указать Id группы или ее код");
	}
}