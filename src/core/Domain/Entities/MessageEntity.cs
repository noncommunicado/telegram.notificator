using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Base;

namespace Domain.Entities;

[Table("message")]
public sealed class MessageEntity : EntityBase
{
	public string Text { get; set; }
	public bool DisableNotification { get; set; } = false;
	public bool? GroupContent { get; set; } = true;
	public ICollection<AttachmentEntity>? Attachments { get; set; }

	public override string ToString() => Text;
}