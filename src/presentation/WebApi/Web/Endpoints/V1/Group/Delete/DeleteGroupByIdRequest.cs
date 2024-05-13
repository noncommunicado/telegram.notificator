
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Web.Endpoints.V1.Group.Delete;

public sealed class DeleteGroupByIdRequest
{
	[FromRoute]
	public Guid GroupId { get; set; }
}