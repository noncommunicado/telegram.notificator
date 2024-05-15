using Domain.Models;

namespace Domain.BotService;

public sealed class SendMessageModel
{
	public SendMessageModel() { }

	public SendMessageModel(long chatId, int threadId, MessageModel message)
	{
		ChatId = chatId;
		ThreadId = threadId;
		Message = message;
	}

	public long ChatId { get; set; }
	public int ThreadId { get; set; }
	public MessageModel Message { get; set; }
}