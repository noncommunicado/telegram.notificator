using System.Security.Claims;
using FastEndpoints.Security;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Services;

public sealed class JwtTokenGenerator
{
	private readonly AuthorizationSettings _settings;

	public JwtTokenGenerator(IOptions<AuthorizationSettings> settings)
	{
		_settings = settings.Value;
	}

	public string GenerateToken(string displayName, string principal, string[] roles)
	{
		return JwtBearer.CreateToken(options => {
			options.SigningKey = _settings.Jwt.SignInKey;
			options.ExpireAt = DateTime.UtcNow.AddDays(_settings.Jwt.LifetimeDays);
			options.User.Roles.AddRange(roles);
			options.User.Claims.Add(new Claim("name", displayName));
			options.User.Claims.Add(new Claim("principal", principal));
		});
	}
}