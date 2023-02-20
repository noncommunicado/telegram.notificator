
using Application.Interfaces;
using AutoMapper;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Groups;

public sealed record GetAllGroupsQuery(long? chatId = null) : IRequest<IEnumerable<GroupDto>>;
public sealed class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, IEnumerable<GroupDto>>
{
	private readonly IMainDbContext _context;
	private readonly IMapper _mapper;
	public GetAllGroupsQueryHandler(IMainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IEnumerable<GroupDto>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
	{
		return await _context.Groups.AsNoTracking()
			.Include(x => x.Members)
			.Where(x => request.chatId == null || (x.Members != null && x.Members.Any(z => z.ChatId == request.chatId)))
			.Select(x => _mapper.Map<GroupDto>(x))
			.ToArrayAsync(cancellationToken);
	}
}