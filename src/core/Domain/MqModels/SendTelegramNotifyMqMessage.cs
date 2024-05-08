
using Domain.Models;

namespace Domain.MqModels;

public sealed class SendTelegramNotifyMqMessage
{
	public SendTelegramNotifyMqMessage()
	{
		
	}

	public SendTelegramNotifyMqMessage(long chatId, Guid messageId)
	{
		ChatId = chatId;
		MessageId = messageId;
	}

	public long ChatId { get; set; }
	public Guid MessageId { get; set; }
}