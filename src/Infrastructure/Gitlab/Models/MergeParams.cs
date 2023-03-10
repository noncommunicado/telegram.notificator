using Newtonsoft.Json;

namespace Gitlab.Models;

public class MergeParams
{
	[JsonProperty("force_remove_source_branch")]
	public string ForceRemoveSourceBranch { get; set; }
}