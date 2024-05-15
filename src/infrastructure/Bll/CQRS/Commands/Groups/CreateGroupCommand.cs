using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Persistence.Contexts;
using Serilog;

namespace Bll.CQRS.Commands.Groups;

public sealed record CreateGroupCommand(string Name, string Code, IEnumerable<GroupMemberEditCreateDto>? Members) : IRequest<Guid>;

public sealed class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Guid>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;

	public CreateGroupCommandHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<Guid> Handle(CreateGroupCommand request, CancellationToken ct)
	{
		var group = new GroupEntity {Id = Guid.NewGuid(), Name = request.Name, SysCode = request.Code};
		var trans = await _context.Database.BeginTransactionAsync(ct);
		try
		{
			await _context.Groups.AddAsync(group, ct);
			await _context.SaveChangesAsync(ct);
			Log.Information("Group created. Id: {GroupId}, Name: {Name}, SysCode: {Code}", group.Id, group.Name,
				group.SysCode);

			// если чаты не были указаны - пропускаем
			if (request.Members == null || request.Members.Any() == false) {
				await trans.CommitAsync(ct);
				return group.Id;
			}

			await _context.GroupMembers.AddRangeAsync(request.Members.Select(x => {
				var entity = _mapper.Map<GroupMemberEntity>(x);
				entity.GroupId = group.Id;
				return entity;
			}), ct);
			await _context.SaveChangesAsync(ct);
			await trans.CommitAsync(ct);
		}
		catch {
			await trans.RollbackAsync(ct);
			throw;
		}
		return group.Id;
	}
}