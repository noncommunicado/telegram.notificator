using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Bll.CQRS.Commands.Groups.GroupMembersCommands;

public sealed record RemoveMembersCommand : ManipulateGroupMembersCommandBase, IRequest;

public sealed class RemoveMembersCommandHandler : IRequestHandler<RemoveMembersCommand>
{
	private readonly MainDbContext _context;
	public RemoveMembersCommandHandler(MainDbContext context)
	{
		_context = context;
	}

	public Task Handle(RemoveMembersCommand request, CancellationToken ct) {
		return new ManipulateGroupMembersCommandBaseHandler(_context).Handle(request, EntityState.Deleted, ct);
	}
}