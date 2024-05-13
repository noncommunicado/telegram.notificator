using Microsoft.AspNetCore.Mvc;

namespace WebApi.Web.Endpoints.V1.Group.GetByCode;

public sealed class GetGroupByCodeRequest
{
	[FromRoute] public string GroupCode { get; set; }
}