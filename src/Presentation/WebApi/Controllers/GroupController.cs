using Application.CQRS.Commands.Groups;
using Application.CQRS.Commands.Groups.GroupMembersCommands;
using Application.CQRS.Queries.Groups;
using AutoMapper;
using Domain.Dto;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Http.Requests;

namespace WebApi.Controllers;

/// <summary>
/// Управление группами рассылки
/// </summary>
[ApiController, Route("api/[controller]")]
public sealed class GroupController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public GroupController(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}

	/// <summary>
	/// Все группы
	/// </summary>
	[HttpGet("All")]
	[SwaggerResponse(StatusCodes.Status200OK, type: typeof(IEnumerable<GroupDto>))]
	public async Task<IActionResult> GetAll([FromQuery] GroupsRequest request, CancellationToken ct) 
		=> Ok(await _mediator.Send(new GetAllGroupsQuery(request.ByChatIdFilter), ct));
	
	/// <summary>
	/// Данные группы с ее участниками
	/// </summary>
	[HttpGet("{groupId}")]
	[SwaggerResponse(StatusCodes.Status200OK, type: typeof(GroupExtendedDto))]
	public async Task<IActionResult> GetAll(int groupId, CancellationToken ct) 
		=> Ok(await _mediator.Send(new GetGroupQuery(groupId), ct));
	
	/// <summary>
	/// Создать новую группу пользователей
	/// </summary>
	[HttpPost]
	[SwaggerResponse(StatusCodes.Status200OK, Type = typeof(int), Description = "Created Id")]
	public async Task<IActionResult> Create(
		[FromBody] CreateGroupRequest request, 
		IValidator<CreateGroupRequest> validator,
		CancellationToken ct)
	{
		await validator.ValidateAndThrowAsync(request, ct);
		var command = _mapper.Map<CreateGroupCommand>(request);
		return Ok(await _mediator.Send(command, ct));
	}
	
	/// <summary>
	/// Обновить группу пользователей, в том числе самих пользователей
	/// </summary>
	/// <remarks>
	/// Полностью обновляет пользователей, удаляет невключенных в новый список и создает новых
	/// </remarks>
	[HttpPut]
	public async Task<IActionResult> Update(
		[FromBody] UpdateGroupRequest request, 
		IValidator<UpdateGroupRequest> validator,
		CancellationToken ct)
	{
		await validator.ValidateAndThrowAsync(request, ct);
		var command = _mapper.Map<UpdateGroupCommand>(request);
		await _mediator.Send(command, ct);
		return Ok();
	}

	
	/// <summary>
	/// Add members to Group
	/// </summary>
	[HttpPut("Members")]
	public async Task<IActionResult> AddMembers(
		[FromBody] AddOrRemoveMembersRequest request,
		IValidator<AddOrRemoveMembersRequest> validator,
		CancellationToken ct
	) {
		await validator.ValidateAndThrowAsync(request, ct);
		return Ok(await _mediator.Send(_mapper.Map<AddMembersCommand>(request), ct));
	}
	
	/// <summary>
	/// Delete members to Group
	/// </summary>
	[HttpDelete("Members")]
	public async Task<IActionResult> DeleteMembers(
		[FromBody] AddOrRemoveMembersRequest request,
		IValidator<AddOrRemoveMembersRequest> validator,
		CancellationToken ct
	) {
		await validator.ValidateAndThrowAsync(request, ct);
		return Ok(await _mediator.Send(_mapper.Map<RemoveMembersCommand>(request), ct));
	}

	/// <summary>
	/// Удалить группу
	/// </summary>
	[HttpDelete("{groupId}")]
	public async Task<IActionResult> Delete([FromRoute] int groupId, CancellationToken ct)
	{
		await _mediator.Send(new DeleteGroupByIdCommand(groupId), ct);
		return Ok();
	}
}