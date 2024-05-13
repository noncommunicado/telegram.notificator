using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Serilog;

namespace Bll.CQRS.Commands.Groups;

public sealed record DeleteGroupByIdCommand(Guid Id) : IRequest;

public sealed class DeleteGroupByIdCommandHandler : IRequestHandler<DeleteGroupByIdCommand>
{
	private readonly MainDbContext _context;
	public DeleteGroupByIdCommandHandler(MainDbContext context)
	{
		_context = context;
	}

	public async Task Handle(DeleteGroupByIdCommand request, CancellationToken ct)
	{
		_context.Groups.Entry(new GroupEntity {Id = request.Id}).State = EntityState.Deleted;
		await _context.SaveChangesAsync(ct);
		Log.Information("Group deleted. Id: {GroupId}", request.Id);
		return;
	}
}