
namespace WebApi.Settings;

public sealed class JwtSettings
{
	public string Issuer { get; set; }
	public string Audience { get; set; }
	public int LifetimeDays { get; set; }
	public string Timezone { get; set; }
	public string SignInKey { get; set; }
}