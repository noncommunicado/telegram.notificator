
using FastEndpoints;

namespace Neftm.Crm.Syncronizer.Api.Endpoints.Auth.AuthTest;

public sealed class AuthTestEndpoint : EndpointWithoutRequest
{
	public override void Configure()
	{
		Get("auth/test");
		Version(1);
		Description(b => b.Produces(401));
		Summary(e => {
			e.Summary = "Test your jwt-token";
			e.Responses[200] = "Authorized";
			e.Responses[401] = "Not authorized";
		});
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		await SendAsync("OK");
	}
}