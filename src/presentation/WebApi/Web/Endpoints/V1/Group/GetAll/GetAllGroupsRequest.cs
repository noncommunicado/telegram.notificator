using Microsoft.AspNetCore.Mvc;

namespace WebApi.Web.Endpoints.V1.Group.GetAll;

public sealed class GetAllGroupsRequest
{
	[FromQuery] public long? ByChatIdFilter { get; set; }
}