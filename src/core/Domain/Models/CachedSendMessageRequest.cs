
using Domain.Entities;

namespace Domain.Models;

public sealed record CachedSendMessageAttachment(Guid Id, string FileName, AttachmentType Type);
public sealed class CachedSendMessageRequest :  MessageBase
{
	public Guid Id { get; set; }

	public List<CachedSendMessageAttachment>? Attachments { get; set; }
}