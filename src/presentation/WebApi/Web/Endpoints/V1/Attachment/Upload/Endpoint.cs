
using Domain.Entities;
using Domain.Helpers;
using Persistence.Contexts;
using Serilog;
using Services;

namespace WebApi.Web.Endpoints.V1.Attachment.Upload;

public sealed class Endpoint : Endpoint<Request, Response>
{
	public IFileService FileService { get; set; }
	public MainDbContext DbContext { get; set; }
	public override void Configure()
	{
		AllowAnonymous();
		Post("attachments/upload");
		Summary(s => {
			s.Summary = "Upload file-attachment to send it later in notification message";
			s.Description = "Don't forget to remind uploaded attachment ID (GUID), it'll be in response!";
		});
		Version(1);
		AllowFileUploads();
		AllowFormData();
	}
	
	public override async Task HandleAsync(Request request, CancellationToken ct)
	{
		var response = new Response();
		foreach (var attachment in request.Attachments) {
			AttachmentEntity entity = new() {
				FileName = attachment.FileName,
				Id = Guid.NewGuid(),
				Type = AttachmentTypeDetector.Detect(attachment.FileName),
				SysCreated = DateTime.Now
			};
			using var transaction = await DbContext.Database.BeginTransactionAsync(ct);

			try {
				await DbContext.Attachments.AddAsync(entity, ct);
				await DbContext.SaveChangesAsync(ct);
				using var stream = attachment.OpenReadStream();
				await FileService.SaveFileAsync(entity.Id, stream, ct);
				response.UploadStates.Add(new(entity.FileName, true, entity.Id));
				await transaction.CommitAsync(ct);
			}
			catch (Exception e) {
				Log.Error(e, "Failed to save file");
				await transaction.RollbackAsync(ct);
				FileService.Remove(entity.Id);
				response.UploadStates.Add(new(entity.FileName, false));
			}
		}
		
		await SendOkAsync(response, ct);
	}
}