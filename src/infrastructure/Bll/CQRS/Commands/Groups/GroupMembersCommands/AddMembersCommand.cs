using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Bll.CQRS.Commands.Groups.GroupMembersCommands;

public sealed record AddMembersCommand : ManipulateGroupMembersCommandBase, IRequest;

public sealed class AddMembersCommandHandler : IRequestHandler<AddMembersCommand>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;

	public AddMembersCommandHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public Task Handle(AddMembersCommand request, CancellationToken ct)
	{
		return new ManipulateGroupMembersCommandBaseHandler(_context, _mapper).Handle(request, EntityState.Added, ct);
	}
}