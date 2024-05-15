using System.ComponentModel.DataAnnotations.Schema;
using Domain.Dto;
using Domain.Entities.Base;

namespace Domain.Entities;

[Table("group_member")]
public class GroupMemberEntity : ICreatedDateTimeEntity
{
	public GroupMemberEntity() { }
	public GroupMemberEntity(long chatId, Guid groupId, int threadId = -1)
	{
		ChatId = chatId;
		GroupId = groupId;
		ThreadId = threadId;
	}

	public Guid GroupId { get; set; }
	public long ChatId { get; set; }

	/// <summary>
	/// Идентификатор топика (темы) в группе с темами
	/// </summary>
	public int ThreadId { get; set; } = -1;
	public virtual GroupEntity Group { get; set; }

	[Column(TypeName = "timestamp without time zone")]
	public DateTime? SysCreated { get; set; } = DateTime.Now;
}