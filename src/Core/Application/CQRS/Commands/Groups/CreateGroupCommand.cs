using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Serilog;

namespace Application.CQRS.Commands.Groups;

public sealed record CreateGroupCommand(string Name, string Code, IEnumerable<long>? Chats) : IRequest<int>;

public sealed class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, int>
{
	private readonly IMainDbContext _context;
	public CreateGroupCommandHandler(IMainDbContext context)
	{
		_context = context;
	}

	public async Task<int> Handle(CreateGroupCommand request, CancellationToken ct)
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