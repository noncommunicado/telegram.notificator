using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Base;

namespace Domain.Entities;

[Table("GroupMember")]
public class GroupMemberEntity : ICreatedDateTimeEntity
{
	public GroupMemberEntity() { }
	public GroupMemberEntity(long chatId, int groupId)
	{
		ChatId = chatId;
		GroupId = groupId;
	}
	
	public long ChatId { get; set; }
	public int GroupId { get; set; }
	public virtual GroupEntity Group { get; set; }
	
	[Column(TypeName = "timestamp without time zone")]
	public DateTime? SysCreated { get; set; } = DateTime.Now;
}