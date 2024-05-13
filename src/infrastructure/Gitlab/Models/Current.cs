using Newtonsoft.Json;

namespace Gitlab.Models;

public class Current
{
	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("username")] public string Username { get; set; }

	[JsonProperty("avatar_url")] public string AvatarUrl { get; set; }

	[JsonProperty("email")] public string Email { get; set; }
}