namespace WebApi.Web;

public sealed class AuthenticationSchemes
{
	public const string StaticApiKey = "StaticApiKey";
	public static string JwtScheme = JwtBearerDefaults.AuthenticationScheme;
}