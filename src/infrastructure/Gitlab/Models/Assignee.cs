using Newtonsoft.Json;

namespace Gitlab.Models;

public class Assignee
{
	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("username")]
	public string Username { get; set; }

	[JsonProperty("avatar_url")]
	public string AvatarUrl { get; set; }

	[JsonProperty("email")]
	public string Email { get; set; }

	[JsonProperty("previous")]
	public List<object> Previous { get; set; }

	[JsonProperty("current")]
	public List<Current> Current { get; set; }
}