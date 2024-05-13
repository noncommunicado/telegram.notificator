using Domain.Models;

namespace Application.Interfaces;

public interface IMessageCache
{
	Task<MessageModel?> GetById(Guid messageId, CancellationToken ct);
	void Set(MessageModel model);
}