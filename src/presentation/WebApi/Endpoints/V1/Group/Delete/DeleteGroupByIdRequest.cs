
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.V1.Group.Delete;

public sealed class DeleteGroupByIdRequest
{
	[FromRoute]
	public int GroupId { get; set; }
}