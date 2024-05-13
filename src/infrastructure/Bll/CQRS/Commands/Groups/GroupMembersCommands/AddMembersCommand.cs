using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Bll.CQRS.Commands.Groups.GroupMembersCommands;

public sealed record AddMembersCommand : ManipulateGroupMembersCommandBase, IRequest;

public sealed class AddMembersCommandHandler : IRequestHandler<AddMembersCommand>
{
	private readonly MainDbContext _context;

	public AddMembersCommandHandler(MainDbContext context)
	{
		_context = context;
	}

	public Task Handle(AddMembersCommand request, CancellationToken ct)
	{
		return new ManipulateGroupMembersCommandBaseHandler(_context).Handle(request, EntityState.Added, ct);
	}
}