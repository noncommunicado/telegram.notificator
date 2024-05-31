
using Domain.Entities;
using Domain.Models;

namespace Domain.BotService;

public sealed record TelegramMessageAttachment(string FileName, AttachmentType Type, Stream FileStream);
public sealed class TelegramMessage : MessageBase
{
	public Guid Id { get; set; }
	public List<TelegramMessageAttachment> Attachments { get; set; }
}