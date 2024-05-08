using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Base;

namespace Domain.Entities;

[Table("Group")]
public class GroupEntity : EntityBase
{
	[StringLength(128)]
	public string Name { get; set; }

	[StringLength(24)]
	public string SysCode { get; set; }

	public virtual ICollection<GroupMemberEntity>? Members { get; set; }
}