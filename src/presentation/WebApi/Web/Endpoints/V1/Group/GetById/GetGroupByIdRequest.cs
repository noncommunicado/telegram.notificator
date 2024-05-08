
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.V1.Group.GetById;

public sealed class GetGroupByIdRequest
{
	[FromRoute]
	public int GroupId { get; set; }
}