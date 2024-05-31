
namespace WebApi.Web.Endpoints.V1.Attachment.Upload;

public sealed class Request
{
	public List<IFormFile> Attachments { get; set; }
}