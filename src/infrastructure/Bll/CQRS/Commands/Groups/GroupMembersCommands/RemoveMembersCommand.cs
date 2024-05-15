using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Bll.CQRS.Commands.Groups.GroupMembersCommands;

public sealed record RemoveMembersCommand : ManipulateGroupMembersCommandBase, IRequest;

public sealed class RemoveMembersCommandHandler : IRequestHandler<RemoveMembersCommand>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;

	public RemoveMembersCommandHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public Task Handle(RemoveMembersCommand request, CancellationToken ct)
	{
		return new ManipulateGroupMembersCommandBaseHandler(_context, _mapper)
			.Handle(request, EntityState.Deleted, ct);
	}
}