using Neftm.Telegram.Notificator.Requests;
using RestSharp;

namespace Neftm.Telegram.Notificator;

public interface INotificatorRepository
{
	Task<RestResponse> SendToChatsAsync(SendToChatsRequest request, CancellationToken ct);
	Task<RestResponse> SendToGroupsAsync(SendToGroupsRequest request, CancellationToken ct);
}