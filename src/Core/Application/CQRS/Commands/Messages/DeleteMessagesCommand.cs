using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Messages;

public sealed record DeleteMessagesCommand(IEnumerable<Guid> Messages) : IRequest;

public sealed class DeleteMessagesCommandHandler : IRequestHandler<DeleteMessagesCommand>
{
	private readonly IMainDbContext _context;
	public DeleteMessagesCommandHandler(IMainDbContext context) {
		_context = context;
	}
	public async Task<Unit> Handle(DeleteMessagesCommand request, CancellationToken cancellationToken)
	{
		foreach (var msgId in request.Messages) {
			_context.Messages.Entry(new MessageEntity {Id = msgId}).State = EntityState.Deleted;
		}
		await _context.SaveChangesAsync(cancellationToken);
		return Unit.Value;
	}
}