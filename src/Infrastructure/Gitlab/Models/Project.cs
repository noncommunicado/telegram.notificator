using Newtonsoft.Json;

namespace Gitlab.Models;

public class Project
{
	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("description")]
	public string Description { get; set; }

	[JsonProperty("web_url")]
	public string WebUrl { get; set; }

	[JsonProperty("avatar_url")]
	public string AvatarUrl { get; set; }

	[JsonProperty("git_ssh_url")]
	public string GitSshUrl { get; set; }

	[JsonProperty("git_http_url")]
	public string GitHttpUrl { get; set; }

	[JsonProperty("namespace")]
	public string Namespace { get; set; }

	[JsonProperty("visibility_level")]
	public int VisibilityLevel { get; set; }

	[JsonProperty("path_with_namespace")]
	public string PathWithNamespace { get; set; }

	[JsonProperty("default_branch")]
	public string DefaultBranch { get; set; }

	[JsonProperty("ci_config_path")]
	public object CiConfigPath { get; set; }

	[JsonProperty("homepage")]
	public string Homepage { get; set; }

	[JsonProperty("url")]
	public string Url { get; set; }

	[JsonProperty("ssh_url")]
	public string SshUrl { get; set; }

	[JsonProperty("http_url")]
	public string HttpUrl { get; set; }
}