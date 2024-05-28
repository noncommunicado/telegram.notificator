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
	public string FileName { get; set; }
	public AttachmentType Type { get; set; }
	
	[NotMapped]
	public string FileExtensionWithDot => Path.GetExtension(FileName).ToLower(CultureInfo.InvariantCulture);

	public ICollection<MessageEntity>? Messages { get; set; }

	public override string ToString() => $"{FileName} ({Type})";
}