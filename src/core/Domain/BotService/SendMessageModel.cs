
using Domain.Models;

namespace Domain.BotService;

public sealed class SendMessageModel
{
	public SendMessageModel()
	{
		
	}

	public SendMessageModel(long chatId, MessageModel message)
	{
		ChatId = chatId;
		Message = message;
	}
	public long ChatId { get; set; }
	public MessageModel Message { get; set; }
}