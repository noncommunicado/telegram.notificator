using Neftm.Telegram.Notificator.Requests;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace Neftm.Telegram.Notificator;

public class NotificatorRepository : INotificatorRepository
{
	private readonly RestClient _client;

	public NotificatorRepository(IHttpClientFactory httpClientFactory)
	{
		_client = new RestClient(httpClientFactory.CreateClient(Constants.HttpClientName), configureSerialization:
			config => { config.UseNewtonsoftJson(); });
	}

	public async Task<RestResponse> SendToChatsAsync(SendToChatsRequest request, CancellationToken ct)
	{
		var httpRequest = new RestRequest("api/telegram/chats/send", Method.Post);
		httpRequest.AddJsonBody(request);
		return await _client.ExecutePostAsync(httpRequest, ct);
	}

	public async Task<RestResponse> SendToGroupsAsync(SendToGroupsRequest request, CancellationToken ct)
	{
		var httpRequest = new RestRequest("api/telegram/groups/send", Method.Post);
		httpRequest.AddJsonBody(request);
		return await _client.ExecutePostAsync(httpRequest, ct);
	}
}