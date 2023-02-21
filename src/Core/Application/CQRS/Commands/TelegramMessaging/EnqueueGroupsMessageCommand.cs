using Application.Interfaces;
using Domain.Models;
using Domain.MqModels;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.TelegramMessaging;

public sealed record EnqueueGroupsMessageCommand(IEnumerable<int> GroupIds, IEnumerable<string> GroupCodes, MessageModel Message) : IRequest;

public sealed class EnqueueGroupsMessageCommandHandler : IRequestHandler<EnqueueGroupsMessageCommand>
{
	private readonly IMainDbContext _context;
	private readonly IPublishEndpoint _pEnd;
	private readonly IMediator _mediator;

	public EnqueueGroupsMessageCommandHandler(
		IPublishEndpoint pEnd,
		IMediator mediator, 
		IMainDbContext context)
	{
		_pEnd = pEnd;
		_mediator = mediator;
		_context = context;
	}
	
	public async Task<Unit> Handle(EnqueueGroupsMessageCommand request, CancellationToken ct)
	{
		var messageId = await _mediator.Send(new CreateMessageCommand(request.Message), ct);
		var groupIds = request.GroupIds.Distinct().ToHashSet();
		var groupCodes = request.GroupCodes.Select(x => x.Trim()).Distinct().ToHashSet();
		var users = await _context.Groups
			.AsNoTrackingWithIdentityResolution()
			.Include(x => x.Members)
			.Where(x => groupIds.Contains(x.Id) || groupCodes.Contains(x.SysCode))
			.SelectMany(x => x.Members)
			.Select(x => x.ChatId)
			.Distinct()
			.ToArrayAsync(ct);

		foreach (var chatId in users)
			await _pEnd.Publish(new SendTelegramNotifyMqMessage(chatId, messageId), ct);

		return Unit.Value;
	}
}