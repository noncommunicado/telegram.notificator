using RestSharp;
using WebApi.Http.Requests;

namespace Neftm.Telegram.Notificator;

public interface INotificatorRepository
{
	Task<RestResponse> SendToChatsAsync(SendToChatsRequest request, CancellationToken ct);
	Task<RestResponse> SendToGroupsAsync(SendToGroupsRequest request, CancellationToken ct);
}