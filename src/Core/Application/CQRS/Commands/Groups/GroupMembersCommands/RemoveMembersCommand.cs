using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Groups.GroupMembersCommands;

public sealed record RemoveMembersCommand : ManipulateGroupMembersCommandBase, IRequest;

public sealed class RemoveMembersCommandHandler : IRequestHandler<RemoveMembersCommand>
{
	private readonly IMainDbContext _context;
	public RemoveMembersCommandHandler(IMainDbContext context)
	{
		_context = context;
	}

	public Task Handle(RemoveMembersCommand request, CancellationToken ct) {
		return new ManipulateGroupMembersCommandBaseHandler(_context).Handle(request, EntityState.Deleted, ct);
	}
}