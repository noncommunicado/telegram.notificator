using AutoMapper;
using Domain.Dto;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Bll.CQRS.Queries.Groups;

public sealed record GetGroupByIdQuery(Guid GroupId) : IRequest<GroupExtendedDto>;

public sealed class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, GroupExtendedDto>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;

	public GetGroupByIdQueryHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<GroupExtendedDto> Handle(GetGroupByIdQuery request, CancellationToken ct)
	{
		var entity = await _context.Groups.AsNoTrackingWithIdentityResolution()
			.Include(x => x.Members.OrderByDescending(x => x.SysCreated))
			.FirstOrDefaultAsync(x => x.Id == request.GroupId, ct);
		if (entity == null) throw new DomainException(StatusCodes.Status404NotFound, "Not found");
		return _mapper.Map<GroupExtendedDto>(entity);
	}
}