using Newtonsoft.Json;

namespace Gitlab.Models;

public class Changes
{
	[JsonProperty("assignees")]
	public Assignee Assignees { get; set; }

	[JsonProperty("total_time_spent")]
	public TotalTimeSpent TotalTimeSpent { get; set; }
}