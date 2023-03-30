
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Messages;


public sealed record DeleteOldMessagesCommand : IRequest;

public sealed class DeleteOldMessagesCommandHandler : IRequestHandler<DeleteOldMessagesCommand>
{
	private readonly IMainDbContext _context;
	private readonly IMediator _mediator;
	public DeleteOldMessagesCommandHandler(IMainDbContext context, IMediator mediator)
	{
		_context = context;
		_mediator = mediator;
	}

	public async Task Handle(DeleteOldMessagesCommand request, CancellationToken cancellationToken)
	{
		var messagesId = await _context.Messages.AsNoTracking()
			.Where(x => x.SysCreated == null || Math.Abs((DateTime.Now - x.SysCreated).Value.TotalHours) > 10)
			.Select(x => x.Id)
			.ToArrayAsync(cancellationToken);
		await _mediator.Send(new DeleteMessagesCommand(messagesId), cancellationToken);
		return;
	}
}