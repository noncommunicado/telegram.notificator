using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Serilog;

namespace Bll.CQRS.Commands.Groups;

public sealed record UpdateGroupCommand(Guid GroupId, string Name, string Code) : IRequest;

public sealed class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
{
	private readonly MainDbContext _context;

	public UpdateGroupCommandHandler(MainDbContext context)
	{
		_context = context;
	}

	public async Task Handle(UpdateGroupCommand request, CancellationToken ct)
	{
		var entity = new GroupEntity {
			Id = request.GroupId, SysCode = request.Code, Name = request.Name
		};
		_context.Groups.Entry(entity).State = EntityState.Modified;
		await _context.SaveChangesAsync(ct);
		Log.Information("Group edited. Id: {GroupId}, Name: {Name}, SysCode: {Code}", entity.Id, entity.Name,
			entity.SysCode);
	}
}