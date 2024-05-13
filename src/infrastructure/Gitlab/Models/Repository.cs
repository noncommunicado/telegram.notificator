using Newtonsoft.Json;

namespace Gitlab.Models;

public class Repository
{
	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("url")] public string Url { get; set; }

	[JsonProperty("description")] public string Description { get; set; }

	[JsonProperty("homepage")] public string Homepage { get; set; }
}