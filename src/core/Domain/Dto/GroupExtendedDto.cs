
namespace Domain.Dto;

public sealed record GroupExtendedDto : GroupDto
{
	public IEnumerable<long> Chats { get; set; }
}