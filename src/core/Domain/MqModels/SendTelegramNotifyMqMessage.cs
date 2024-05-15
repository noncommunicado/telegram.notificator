namespace Domain.MqModels;

public sealed class SendTelegramNotifyMqMessage
{
	public SendTelegramNotifyMqMessage() { }

	public SendTelegramNotifyMqMessage(long chatId, int threadId, Guid messageId)
	{
		ChatId = chatId;
		MessageId = messageId;
		ThreadId = threadId;
	}

	public long ChatId { get; set; }
	public int ThreadId { get; set; }
	public Guid MessageId { get; set; }
}