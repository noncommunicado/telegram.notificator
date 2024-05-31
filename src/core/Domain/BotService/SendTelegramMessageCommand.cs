using Domain.Models;

namespace Domain.BotService;

public sealed class SendTelegramMessageCommand
{
	public SendTelegramMessageCommand() { }

	public SendTelegramMessageCommand(long chatId, int threadId, TelegramMessage message)
	{
		ChatId = chatId;
		ThreadId = threadId;
		Message = message;
	}

	public long ChatId { get; set; }
	public int ThreadId { get; set; }
	public TelegramMessage Message { get; set; }
}