using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Web;

public sealed class AuthSchemes
{
	public const string StaticApiKey = "StaticApiKey";
	public static string JwtScheme = JwtBearerDefaults.AuthenticationScheme;
}