using AutoMapper;
using Domain.Dto;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Bll.CQRS.Queries.Groups;

public sealed record GetGroupQuery(Guid GroupId) : IRequest<GroupExtendedDto>;

public sealed class GetGroupQueryHandler : IRequestHandler<GetGroupQuery, GroupExtendedDto>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;

	public GetGroupQueryHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<GroupExtendedDto> Handle(GetGroupQuery request, CancellationToken ct)
	{
		var entity = await _context.Groups.AsNoTrackingWithIdentityResolution()
			.Include(x => x.Members)
			.FirstOrDefaultAsync(x => x.Id == request.GroupId, ct);
		if (entity == null) throw new DomainException(StatusCodes.Status404NotFound, "Not found");
		return _mapper.Map<GroupExtendedDto>(entity);
	}
}