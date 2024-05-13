using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using MediatR;
using Persistence.Contexts;
using Serilog;

namespace Bll.CQRS.Commands.TelegramMessaging;

public sealed record CreateMessageCommand(MessageModel Message) : IRequest<Guid>;

public sealed class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Guid>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;
	private readonly IMessageCache _messageCache;
	public CreateMessageCommandHandler(MainDbContext context, IMapper mapper, IMessageCache messageCache)
	{
		_context = context;
		_mapper = mapper;
		_messageCache = messageCache;
	}
	public async Task<Guid> Handle(CreateMessageCommand request, CancellationToken ct)
	{
		request.Message.Id = Guid.NewGuid();
		Log.Information("New message registration {MessageId}. Message text: {Text}", request.Message.Id, request.Message.Text);

		var entity = _mapper.Map<MessageEntity>(request.Message);
		await _context.Messages.AddAsync(entity, ct);
		await _context.SaveChangesAsync(ct);
		_messageCache.Set(request.Message);
		return request.Message.Id;
	}
}