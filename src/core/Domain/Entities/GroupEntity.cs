using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Base;

namespace Domain.Entities;

[Table("group")]
public class GroupEntity : EntityBase
{
	public string Name { get; set; }

	public string SysCode { get; set; }

	public virtual ICollection<GroupMemberEntity>? Members { get; set; }

	public override string ToString() => $"{Name} ({SysCode})";
}