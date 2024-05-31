using Domain.BotService;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Serilog;
using Services;
using Telegram.Bot.Exceptions;

namespace Bll.CQRS.Commands.TelegramMessaging;

public sealed record SendMessageCommand(long ChatId, int ThreadId, Guid MessageId, List<Guid> AttachmentsIds) : IRequest;

public sealed class SendNotifyCommandHandler : IRequestHandler<SendMessageCommand>
{
	private readonly IBotHostedService _botService;
	private readonly IFileService _fileService;
	private readonly MainDbContext _context;

	public SendNotifyCommandHandler(
		IBotHostedService botService, IFileService fileService, MainDbContext context)
	{
		_botService = botService;
		_fileService = fileService;
		_context = context;
	}

	public async Task Handle(SendMessageCommand request, CancellationToken ct)
	{
		var dbMessage = await _context.Messages.FindAsync(request.MessageId, ct);
		if (dbMessage is null) {
			throw new Exception("Message not found in database");
		}

		var attachments = new List<TelegramMessageAttachment>(request.AttachmentsIds.Count);
		foreach (var a in request.AttachmentsIds) {
			var dbAttachment = await _context.Attachments.FindAsync(a, ct);
			if (dbAttachment is null) continue;
			var streamResult = _fileService.GetFileStream(dbAttachment.Id, ct);
			if (streamResult.Exist is false) continue;
			attachments.Add(new TelegramMessageAttachment(dbAttachment.FileName, dbAttachment.Type, streamResult.FileStream));
		}

		var messageToSend = new TelegramMessage {
			Id = dbMessage.Id,
			GroupContent = dbMessage.GroupContent,
			DisableNotification = dbMessage.DisableNotification,
			Text = dbMessage.Text,
			Attachments = attachments
		};
		Log.Information("Sending Telegram message {MessageId} to chat {ChatId}, thread {ThreadId}", request.MessageId, request.ChatId, request.ThreadId);
		try {
			await _botService.SendAsync(new SendTelegramMessageCommand(request.ChatId, request.ThreadId, messageToSend), ct: ct);
		}
		catch (ApiRequestException arex) when (arex.ErrorCode == 400) {
			// dont throw - because nothing to do with it - swallow
			Log.Error(arex, "Telegram-Bot don't has chat with this chatid");
		}
		catch (ApiRequestException arex) when (arex.ErrorCode is 420 or >= 500) {
			// must be thrown for RMQ retry
			Log.Error(arex, "Telegram API is currently unavailable or overloaded");
			throw;
		}
	}
}