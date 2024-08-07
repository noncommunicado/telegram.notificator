
namespace WebApi.Models;


public sealed class SecuritySettings
{
	/// <summary>
	/// Is global JWT security of Application is enabled 
	/// </summary>
	public bool Enabled { get; init; } = false;

	/// <summary>
	/// Jwt token secret key for signing JWT access token
	/// </summary>
	public string SignInKey { get; init; }

	/// <summary>
	/// How many days will token live before expiration
	/// </summary>
	public int LifetimeDays { get; init; } = 180; // default 6 month 

	public LdapConfiguration? LdapSecurity { get; init; }
	
	public sealed class LdapConfiguration
	{
		public required string BaseUrl { get; init; }
	}
}

