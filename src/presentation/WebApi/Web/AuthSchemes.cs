
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Web;

public sealed class AuthSchemes
{
	public static string JwtScheme = JwtBearerDefaults.AuthenticationScheme;
	public const string StaticApiKey = "StaticApiKey";
}