using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Domain.Entities.Base;

namespace Domain.Entities;

[Table("message")]
public sealed class MessageEntity : EntityBase
{
	[AllowNull]
	public string? Text { get; set; }
	public bool DisableNotification { get; set; } = false;
	public bool? GroupContent { get; set; } = true;

	public override string ToString() => Text;
}