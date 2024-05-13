using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Bll.CQRS.Commands.Messages;

public sealed record DeleteMessagesCommand(IEnumerable<Guid> Messages) : IRequest;

public sealed class DeleteMessagesCommandHandler : IRequestHandler<DeleteMessagesCommand>
{
	private readonly MainDbContext _context;

	public DeleteMessagesCommandHandler(MainDbContext context)
	{
		_context = context;
	}

	public async Task Handle(DeleteMessagesCommand request, CancellationToken cancellationToken)
	{
		foreach (var msgId in request.Messages)
			_context.Messages.Entry(new MessageEntity {Id = msgId}).State = EntityState.Deleted;
		await _context.SaveChangesAsync(cancellationToken);
	}
}