using Application.CQRS.Commands.TelegramMessaging;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Http.Requests;

namespace WebApi.Controllers;

/// <summary>
/// Отправка сообщений в Telegram
/// </summary>
[ApiController, Route("[controller]")]
public sealed class TelegramController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;
	public TelegramController(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	/// <summary>
	/// Отправить сообщение конкретным чатам
	/// </summary>
	[HttpPost("Chats/Send")]
	[SwaggerResponse(StatusCodes.Status200OK, description: "Поставлено в очередь на отправку")]
	public async Task<IActionResult> SendToChats(
		[FromBody] SendToChatsRequest request,
		IValidator<SendToChatsRequest> validator,
		CancellationToken ct)
	{
		await validator.ValidateAndThrowAsync(request, ct);
		var command = _mapper.Map<EnqueueChatsMessageCommand>(request);
		await _mediator.Send(command, ct);
		return Ok();
	}
	
	/// <summary>
	/// Отправить сообщение всем чатам в группах, если есть пересечение - отправится всего одно сообщение
	/// </summary>
	[HttpPost("Groups/Send")]
	[SwaggerResponse(StatusCodes.Status200OK, description: "Поставлено в очередь на отправку")]
	public async Task<IActionResult> SendToGroups(
		[FromBody] SendToGroupsRequest request,
		IValidator<SendToGroupsRequest> validator,
		CancellationToken ct
	) {
		await validator.ValidateAndThrowAsync(request, ct);
		var command = _mapper.Map<EnqueueGroupsMessageCommand>(request);
		await _mediator.Send(command, ct);
		return Ok();
	}
}