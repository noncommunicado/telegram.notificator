using Domain.Models;
using Domain.MqModels;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Bll.CQRS.Commands.TelegramMessaging;

public sealed record EnqueueGroupsMessageCommand(
	IEnumerable<Guid> GroupIds,
	IEnumerable<string> GroupCodes,
	MessageModel Message) : IRequest;

public sealed class EnqueueGroupsMessageCommandHandler : IRequestHandler<EnqueueGroupsMessageCommand>
{
	private readonly MainDbContext _context;
	private readonly IMediator _mediator;
	private readonly IPublishEndpoint _pEnd;

	public EnqueueGroupsMessageCommandHandler(
		IPublishEndpoint pEnd,
		IMediator mediator,
		MainDbContext context)
	{
		_pEnd = pEnd;
		_mediator = mediator;
		_context = context;
	}

	public async Task Handle(EnqueueGroupsMessageCommand request, CancellationToken ct)
	{
		var messageId = await _mediator.Send(new CreateMessageCommand(request.Message), ct);
		var groupIds = request.GroupIds.Distinct().ToHashSet();
		var groupCodes = request.GroupCodes.Select(x => x.Trim()).Distinct().ToHashSet();
		var users = await _context.Groups
			.AsNoTrackingWithIdentityResolution()
			.Include(x => x.Members)
			.Where(x => groupIds.Contains(x.Id) || groupCodes.Contains(x.SysCode))
			.SelectMany(x => x.Members)
			.Select(x => new {x.ChatId, x.ThreadId})
			.Distinct()
			.ToArrayAsync(ct);

		foreach (var chat in users)
			await _pEnd.Publish(new SendTelegramNotifyMqMessage(chat.ChatId, chat.ThreadId, messageId), ct);
	}
}