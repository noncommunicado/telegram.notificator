
namespace WebApi.Endpoints.V1.Auth.GenerateToken;

public sealed class GenerateTokenBySecretKeyRequestEndpoint : Endpoint<GenerateTokenBySecretKeyRequest>
{
	public override void Configure()
	{
		AllowAnonymous();
		Put("auth/token/by/secret");
		Version(1);
	}
}