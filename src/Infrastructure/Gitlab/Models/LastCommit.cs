using Newtonsoft.Json;

namespace Gitlab.Models;

public class LastCommit
{
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("message")]
	public string Message { get; set; }

	[JsonProperty("title")]
	public string Title { get; set; }

	[JsonProperty("timestamp")]
	public DateTime Timestamp { get; set; }

	[JsonProperty("url")]
	public string Url { get; set; }

	[JsonProperty("author")]
	public Author Author { get; set; }
}