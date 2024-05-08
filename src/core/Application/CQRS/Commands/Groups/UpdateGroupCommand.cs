using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.CQRS.Commands.Groups;

public sealed record UpdateGroupCommand(int Id, string Name, string Code, IEnumerable<long>? Chats) : IRequest;

public sealed class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
{
	private readonly IMainDbContext _context;
	public UpdateGroupCommandHandler(IMainDbContext context)
	{
		_context = context;
	}
	public async Task Handle(UpdateGroupCommand request, CancellationToken ct)
	{
		var entity = new GroupEntity
		{
			Id = request.Id, SysCode = request.Code, Name = request.Name
		};
		_context.Groups.Entry(entity).State = EntityState.Modified;
		await _context.SaveChangesAsync(ct);
		Log.Information("Group edited. Id: {GroupId}, Name: {Name}, SysCode: {Code}", entity.Id, entity.Name, entity.SysCode);
		
		// если чаты не были указаны - пропускаем
		if(request.Chats == null || request.Chats.Any() == false)
			return;
		
		var requestChats = request.Chats.ToHashSet();
		var dbChats = await _context.GroupMembers.Where(x => x.GroupId == request.Id).ToArrayAsync(ct);
		foreach (var chat in dbChats)
		{
			if (requestChats.Contains(chat.ChatId))
				continue;
			_context.GroupMembers.Remove(chat);
		}

		await _context.GroupMembers.AddRangeAsync(
			requestChats.Except(dbChats.Select(x => x.ChatId)).Select(x => new GroupMemberEntity
			{
				ChatId = x, GroupId = request.Id
			}), ct);
		
		await _context.SaveChangesAsync(ct);
		return;
	}
}