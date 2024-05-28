
using System.Globalization;
using Domain.Entities;

namespace Domain.Dto;

public sealed class AttachmentDto
{
	public Guid Id { get; init; }
	public string FileName { get; init; }
	public AttachmentType Type { get; init; }
	
	public string FileExtensionWithDot
		=> Path.GetExtension(FileName).ToLower(CultureInfo.InvariantCulture);
}