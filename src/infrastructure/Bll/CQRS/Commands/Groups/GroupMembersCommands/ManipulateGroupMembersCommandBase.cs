using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Bll.CQRS.Commands.Groups.GroupMembersCommands;

public record ManipulateGroupMembersCommandBase : IRequest
{
	public Guid? GroupId { get; init; }
	public string? GroupCode { get; init; }
	public IEnumerable<long> Chats { get; init; }
}

public sealed class ManipulateGroupMembersCommandBaseHandler
{
	private readonly MainDbContext _context;

	public ManipulateGroupMembersCommandBaseHandler(MainDbContext context)
	{
		_context = context;
	}

	public async Task Handle(ManipulateGroupMembersCommandBase request, EntityState entityState, CancellationToken ct)
	{
		await using var transaction = await _context.Database.BeginTransactionAsync(ct);
		try {
			var groupTuple = await GetGroupChatsAsync(request, ct);
			if (groupTuple.groupId == null)
				throw new DomainException(404, "Gorup not found");

			var chatsToEdit = entityState switch {
				EntityState.Added => request.Chats.Except(groupTuple.dbChats),
				EntityState.Deleted => request.Chats.Intersect(groupTuple.dbChats),
				_ => throw new ArgumentOutOfRangeException(nameof(entityState), entityState,
					"Action with input state not supported")
			};

			foreach (var chat in chatsToEdit)
				_context.GroupMembers
					.Entry(new GroupMemberEntity(chat, groupTuple.groupId.Value)).State = entityState;

			await _context.SaveChangesAsync(ct);
			await transaction.CommitAsync(ct);
		}
		catch (Exception e) {
			await transaction.RollbackAsync(ct);
			throw;
		}
	}

	private async Task<(Guid? groupId, IEnumerable<long> dbChats)> GetGroupChatsAsync(
		ManipulateGroupMembersCommandBase request, CancellationToken ct)
	{
		if (request.GroupId.HasValue) {
			// search by id
			var group = await _context.Groups
				.AsNoTrackingWithIdentityResolution()
				.Include(x => x.Members)
				.FirstOrDefaultAsync(x => x.Id == request.GroupId, ct);
			if (group == null) return (null, ArraySegment<long>.Empty);
			return (group.Id, (group.Members ?? ArraySegment<GroupMemberEntity>.Empty).Select(x => x.ChatId));
		}

		// throw if no presented id either code 
		if (string.IsNullOrEmpty(request.GroupCode))
			throw new DomainException(StatusCodes.Status400BadRequest, "Нужно указать Id группы или ее код");

		// search by code
		{
			var group = await _context.Groups
				.AsNoTrackingWithIdentityResolution()
				.Include(x => x.Members)
				.FirstOrDefaultAsync(x => x.SysCode == request.GroupCode, ct);
			if (group == null) return (null, ArraySegment<long>.Empty);
			return (group.Id, (group.Members ?? ArraySegment<GroupMemberEntity>.Empty).Select(x => x.ChatId));
		}
	}
}