using AutoMapper;
using Domain.Dto;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Bll.CQRS.Queries.Groups;

public sealed record GetGroupByCodeQuery(string Code) : IRequest<GroupExtendedDto>;

public sealed class GetGroupByCodeQueryHandler : IRequestHandler<GetGroupByCodeQuery, GroupExtendedDto>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;

	public GetGroupByCodeQueryHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<GroupExtendedDto> Handle(GetGroupByCodeQuery request, CancellationToken ct)
	{
		var entity = await _context.Groups.AsNoTrackingWithIdentityResolution()
			.Include(x => x.Members)
			.FirstOrDefaultAsync(x => x.SysCode == request.Code, ct);
		if (entity == null) throw new DomainException(StatusCodes.Status404NotFound, "Not found");
		return _mapper.Map<GroupExtendedDto>(entity);
	}
}