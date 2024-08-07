using FastEndpoints.Security;
using KutCode.Security.Ldap.Models;
using Microsoft.Extensions.Options;
using WebApi.Settings;
using WebApi.Static;

namespace WebApi.Services;

public sealed class JwtTokenGenerator
{
	private readonly IOptions<AuthorizationSettings> _settings;
	public JwtTokenGenerator(IOptions<AuthorizationSettings> settings)
	{
		_settings = settings;
	}

	public string GenerateToken(LdapUserData ldapUserData)
	{
		return JWTBearer.CreateToken(
			signingKey: _settings.Value.Jwt.SignInKey,
			expireAt: DateTime.UtcNow.AddDays(_settings.Value.Jwt.LifetimeDays),
			audience: _settings.Value.Jwt.Audience,
			issuer: _settings.Value.Jwt.Issuer,
			privileges: u =>
			{
				u.Roles.AddRange(ldapUserData.MemberOfGroups);
				u.Claims.Add(new(ClaimNames.Name, ldapUserData.UserDisplayName));
				u.Claims.Add(new(ClaimNames.EMail, ldapUserData.UserEmail));
			});
	}
}