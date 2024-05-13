using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Base;

namespace Domain.Entities;

[Table("message")]
public sealed class MessageEntity : ICreatedDateTimeEntity
{
	[Key] public Guid Id { get; set; }

	public string Text { get; set; }

	/// <summary>
	///     Is send without notification (push & sound)
	/// </summary>
	public bool DisableNotification { get; set; } = false;

	[Column(TypeName = "timestamp without time zone")]
	public DateTime? SysCreated { get; set; } = DateTime.Now;
}