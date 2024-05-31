using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Serilog;

namespace Bll.CQRS.Commands.TelegramMessaging;

public sealed record CreateMessageCommand(SendMessageRequest SendCreatedMessage) : IRequest<Guid>;

public sealed class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Guid>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;

	public CreateMessageCommandHandler(MainDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<Guid> Handle(CreateMessageCommand request, CancellationToken ct)
	{
		var entity = _mapper.Map<MessageEntity>(request.SendCreatedMessage);
		entity.Id = Guid.NewGuid();
		Log.Information("New message registration {MessageId}. Message text: {Text}",
			entity.Id, request.SendCreatedMessage.Text);
		await _context.Messages.AddAsync(entity, ct);
		await _context.SaveChangesAsync(ct);
		return entity.Id;
	}
}