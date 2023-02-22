
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers;

/// <summary>
/// Уведомления в Gitlab
/// </summary>
[ApiController, Route("api/[controller]")]
public sealed class GitlabController : ControllerBase
{
	//[Route("Merge")]
	[AcceptVerbs("POST", "GET", "PUT")]
	public async Task<IActionResult> MergeCreated(CancellationToken ct)
	{
		var streamReader = new StreamReader(Request.Body);
		var content = await streamReader.ReadToEndAsync(ct);
		Log.Information("Gitlab incoming request.{NewLine}{Content}", Environment.NewLine, content);
		
		return Ok();
	}
}