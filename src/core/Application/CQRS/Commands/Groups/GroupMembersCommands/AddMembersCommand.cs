using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Groups.GroupMembersCommands;

public sealed record AddMembersCommand : ManipulateGroupMembersCommandBase, IRequest;

public sealed class AddMembersCommandHandler : IRequestHandler<AddMembersCommand>
{
	private readonly IMainDbContext _context;
	public AddMembersCommandHandler(IMainDbContext context) {
		_context = context;
	}
	public Task Handle(AddMembersCommand request, CancellationToken ct) {
		return new ManipulateGroupMembersCommandBaseHandler(_context).Handle(request, EntityState.Added, ct);
	}
}