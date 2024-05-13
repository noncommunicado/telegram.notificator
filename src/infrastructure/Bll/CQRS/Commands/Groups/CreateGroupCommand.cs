using Domain.Entities;
using MediatR;
using Persistence.Contexts;
using Serilog;

namespace Bll.CQRS.Commands.Groups;

public sealed record CreateGroupCommand(string Name, string Code, IEnumerable<long>? Chats) : IRequest<Guid>;

public sealed class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Guid>
{
	private readonly MainDbContext _context;
	public CreateGroupCommandHandler(MainDbContext context)
	{
		_context = context;
	}

	public async Task<Guid> Handle(CreateGroupCommand request, CancellationToken ct)
	{
		var group = new GroupEntity { Name = request.Name, SysCode = request.Code };

		await _context.Groups.AddAsync(group, ct);
		await _context.SaveChangesAsync(ct);
		Log.Information("Group created. Id: {GroupId}, Name: {Name}, SysCode: {Code}", group.Id, group.Name, group.SysCode);
		
		// если чаты не были указаны - пропускаем
		if(request.Chats == null || request.Chats.Any() == false)
			return group.Id;

		await _context.GroupMembers.AddRangeAsync(request.Chats.Select(x => new GroupMemberEntity(x, group.Id)), ct);
		await _context.SaveChangesAsync(ct);
		return group.Id;
	}
}