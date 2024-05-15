namespace Domain.Dto;

public sealed record GroupExtendedDto : GroupDto
{
	public IEnumerable<GroupMemberDto> Members { get; set; }
}