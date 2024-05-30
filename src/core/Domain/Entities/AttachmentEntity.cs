using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Domain.Entities.Base;

namespace Domain.Entities;

public enum AttachmentType
{
	Photo, File
}

[Table("attachment")]
public sealed class AttachmentEntity : EntityBase
{
	public AttachmentEntity(Guid id) : base(id) { }
	public AttachmentEntity() { }

	public string FileName { get; set; }
	public AttachmentType Type { get; set; }

	public override string ToString() => $"{FileName} ({Type})";
}