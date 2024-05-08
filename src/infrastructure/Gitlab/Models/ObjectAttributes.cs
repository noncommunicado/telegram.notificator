using Newtonsoft.Json;

namespace Gitlab.Models;

public class ObjectAttributes
{
	[JsonProperty("assignee_id")]
	public int AssigneeId { get; set; }

	[JsonProperty("author_id")]
	public int AuthorId { get; set; }

	[JsonProperty("created_at")]
	public DateTime CreatedAt { get; set; }

	[JsonProperty("description")]
	public string Description { get; set; }

	[JsonProperty("head_pipeline_id")]
	public object HeadPipelineId { get; set; }

	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("iid")]
	public int Iid { get; set; }

	[JsonProperty("last_edited_at")]
	public object LastEditedAt { get; set; }

	[JsonProperty("last_edited_by_id")]
	public object LastEditedById { get; set; }

	[JsonProperty("merge_commit_sha")]
	public string MergeCommitSha { get; set; }

	[JsonProperty("merge_error")]
	public object MergeError { get; set; }

	[JsonProperty("merge_params")]
	public MergeParams MergeParams { get; set; }

	[JsonProperty("merge_status")]
	public string MergeStatus { get; set; }

	[JsonProperty("merge_user_id")]
	public object MergeUserId { get; set; }

	[JsonProperty("merge_when_pipeline_succeeds")]
	public bool MergeWhenPipelineSucceeds { get; set; }

	[JsonProperty("milestone_id")]
	public object MilestoneId { get; set; }

	[JsonProperty("source_branch")]
	public string SourceBranch { get; set; }

	[JsonProperty("source_project_id")]
	public int SourceProjectId { get; set; }

	[JsonProperty("state_id")]
	public int StateId { get; set; }

	[JsonProperty("target_branch")]
	public string TargetBranch { get; set; }

	[JsonProperty("target_project_id")]
	public int TargetProjectId { get; set; }

	[JsonProperty("time_estimate")]
	public int TimeEstimate { get; set; }

	[JsonProperty("title")]
	public string Title { get; set; }

	[JsonProperty("updated_at")]
	public DateTime UpdatedAt { get; set; }

	[JsonProperty("updated_by_id")]
	public object UpdatedById { get; set; }

	[JsonProperty("url")]
	public string Url { get; set; }

	[JsonProperty("source")]
	public Source Source { get; set; }

	[JsonProperty("target")]
	public Target Target { get; set; }

	[JsonProperty("last_commit")]
	public LastCommit LastCommit { get; set; }

	[JsonProperty("work_in_progress")]
	public bool WorkInProgress { get; set; }

	[JsonProperty("total_time_spent")]
	public int TotalTimeSpent { get; set; }

	[JsonProperty("human_total_time_spent")]
	public object HumanTotalTimeSpent { get; set; }

	[JsonProperty("human_time_estimate")]
	public object HumanTimeEstimate { get; set; }

	[JsonProperty("assignee_ids")]
	public List<int> AssigneeIds { get; set; }

	[JsonProperty("state")]
	public string State { get; set; }
}