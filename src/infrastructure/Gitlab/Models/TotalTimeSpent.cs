using Newtonsoft.Json;

namespace Gitlab.Models;

public class TotalTimeSpent
{
	[JsonProperty("previous")]
	public object Previous { get; set; }

	[JsonProperty("current")]
	public int Current { get; set; }
}