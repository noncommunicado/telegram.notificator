
namespace WebApi.Web.Endpoints.V1.Attachment.Upload;

public sealed class Response
{
	public List<AttachmentUploadState> UploadStates { get; set; } = new();
	public sealed record AttachmentUploadState(string FileName, bool Success, Guid? Id = null);
}