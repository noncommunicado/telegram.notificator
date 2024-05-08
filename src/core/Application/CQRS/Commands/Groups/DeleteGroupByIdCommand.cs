using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.CQRS.Commands.Groups;

public sealed record DeleteGroupByIdCommand(int Id) : IRequest;

public sealed class DeleteGroupByIdCommandHandler : IRequestHandler<DeleteGroupByIdCommand>
{
	private readonly IMainDbContext _context;
	public DeleteGroupByIdCommandHandler(IMainDbContext context)
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