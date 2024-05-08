
using Gitlab.Models;
using Newtonsoft.Json;

namespace Gitlab.Requests;

public sealed class MergeRequestRequest
{
	[JsonProperty("object_kind")]
	public string ObjectKind { get; set; }

	[JsonProperty("event_type")]
	public string EventType { get; set; }

	[JsonProperty("user")]
	public GitlabUser User { get; set; }

	[JsonProperty("project")]
	public Project Project { get; set; }

	[JsonProperty("object_attributes")]
	public ObjectAttributes ObjectAttributes { get; set; }

	[JsonProperty("labels")]
	public List<object> Labels { get; set; }

	[JsonProperty("changes")]
	public Changes Changes { get; set; }

	[JsonProperty("repository")]
	public Repository Repository { get; set; }

	[JsonProperty("assignees")]
	public List<Assignee> Assignees { get; set; }
}