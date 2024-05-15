
namespace Domain.Dto;


public record GroupMemberEditCreateDto
{
	public long ChatId { get; init; }

	/// <summary>
	/// Идентификатор топика
	/// </summary>
	public int ThreadId { get; init; } = -1;
}  

public sealed record GroupMemberDto : GroupMemberEditCreateDto
{
	public Guid GroupId { get; init; }
	public DateTime? SysCreated { get; init; }
}

public sealed class GroupMemberDtoEqulityComparer : IEqualityComparer<GroupMemberDto>
{
	public bool Equals(GroupMemberDto x, GroupMemberDto y)
	{
		if (ReferenceEquals(x, y)) return true;
		if (ReferenceEquals(x, null)) return false;
		if (ReferenceEquals(y, null)) return false;
		if (x.GetType() != y.GetType()) return false;
		return x.ChatId == y.ChatId && x.ThreadId == y.ThreadId;
	}

	public int GetHashCode(GroupMemberDto obj)
	{
		return HashCode.Combine(obj.ChatId, obj.ThreadId);
	}
}

