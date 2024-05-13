namespace WebApi.Settings;

public sealed class AuthorizationSettings
{
	public bool Enabled { get; set; } = false;
	public JwtSettings Jwt { get; set; }
	public StaticKeySettings Static { get; set; }
}