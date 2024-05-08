
namespace WebApi.Endpoints.V1.Auth.GenerateToken;

public sealed class GenerateTokenBySecretKeyRequestEndpoint : Endpoint<GenerateTokenBySecretKeyRequest>
{
	public override void Configure()
	{
		AllowAnonymous();
		Put("auth/token/by/secret");
		Version(1);
	}

	public override async Task HandleAsync(GenerateTokenBySecretKeyRequest req, CancellationToken ct)
	{
		await SendOkAsync(ct);
	}
}