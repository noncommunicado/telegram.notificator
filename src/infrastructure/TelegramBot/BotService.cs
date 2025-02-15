using Domain.BotService;
using Domain.Entities;
using Domain.Interfaces;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Cache;
using TelegramBot.UpdateHandlers;

namespace TelegramBot;

public sealed class BotService : IBotHostedService
{
	private readonly BaseUpdateHandler _baseUpdateHandler = new();
	private readonly TelegramBotOptions _options;
	public TelegramBotClient BotClient { get; private set; }

	public BotService(TelegramBotOptions options)
	{
		_options = options;
		if (options.IsEnabled is false) return;
		ArgumentException.ThrowIfNullOrWhiteSpace(options.Token, "Telegram bot token");
		BotClient = new TelegramBotClient(options.Token);
	}

	public async Task SendAsync(SendTelegramMessageCommand request, CancellationToken ct = default)
	{
		if (request.Message.Attachments.Count > 0) {
			try {
				await SendMessageWithAttachmentsAsync(request, ct);
			}
			finally {
				foreach (var a in request.Message.Attachments)
					await a.FileStream.DisposeAsync();
			}
		}
		else if (string.IsNullOrWhiteSpace(request.Message.Text) is false) { 
			// sending without attachments, just text msg (4000 symb. limit)
			await SendSimpleTextAsync(request, ct).ConfigureAwait(false);
		}
		else {
			Log.Warning("Message {Uid} hasn't any attachments, text is empty also!", request.Message.Id);
			return;
		}
		Log.Information("Message sent {Uid}", request.Message.Id);
	}

	private async Task SendMessageWithAttachmentsAsync(SendTelegramMessageCommand request, CancellationToken ct)
	{
		bool hastText = string.IsNullOrEmpty(request.Message.Text) is false;
		bool sendCaption = request.Message.Attachments.Count == 1 && hastText;
		
		// отправляем контент группами
		if (request.Message.GroupContent is true) {
			if (hastText)
				await SendSimpleTextAsync(request, ct).ConfigureAwait(false);
			await SendGroupedMediaTextAsync(request, AttachmentType.Photo, ct).ConfigureAwait(false);
            await SendGroupedMediaTextAsync(request, AttachmentType.File, ct).ConfigureAwait(false);
		}
		// отправляем контент по одному, если attachment всего 1, добавляем caption
		else {
			foreach (var a in request.Message.Attachments.OrderBy(x => x.Type)) {
				if (a.Type == AttachmentType.Photo)
					await BotClient.SendPhotoAsync(
						request.ChatId, 
						new InputFileStream(a.FileStream, a.FileName),
						caption: sendCaption ? request.Message.Text : null,
						disableNotification: request.Message.DisableNotification,
						parseMode: ParseMode.Html,
						messageThreadId: request.ThreadId < 0 ? null : request.ThreadId
					).ConfigureAwait(false);
				else if (a.Type == AttachmentType.File)
					await BotClient.SendDocumentAsync(
						request.ChatId, 
						new InputFileStream(a.FileStream, a.FileName),
						disableNotification: request.Message.DisableNotification,
						messageThreadId: request.ThreadId < 0 ? null : request.ThreadId,
						parseMode: ParseMode.Html,
						caption: sendCaption ? request.Message.Text : null
					).ConfigureAwait(false);
			}
			if (sendCaption is false)
				await SendSimpleTextAsync(request, ct).ConfigureAwait(false);
		}
	}

	private Task SendSimpleTextAsync(SendTelegramMessageCommand request, CancellationToken ct = default) =>
		BotClient.SendTextMessageAsync(
			request.ChatId,
			request.Message.Text,
			messageThreadId: request.ThreadId < 0 ? null : request.ThreadId,
			parseMode: ParseMode.Html,
			disableNotification: request.Message.DisableNotification,
			cancellationToken: ct
		);
	
	private Task SendGroupedMediaTextAsync(SendTelegramMessageCommand request, AttachmentType type, CancellationToken ct = default)
	{
		var media = new List<IAlbumInputMedia>();
		foreach (var a in request.Message.Attachments) {
			if (a.Type != type) continue;
			switch (type) {
				case AttachmentType.File:
					media.Add(new InputMediaDocument(new InputFileStream(a.FileStream, a.FileName))); 
					break;
				case AttachmentType.Photo:
					media.Add(new InputMediaPhoto(new InputFileStream(a.FileStream, a.FileName))); 
					break;
			}
		}

		if (media.Count == 0) return Task.CompletedTask;
		return BotClient.SendMediaGroupAsync(request.ChatId, media,
			disableNotification: request.Message.DisableNotification,
			messageThreadId: request.ThreadId < 0 ? null : request.ThreadId
		);
	}

	#region tech

	public async Task StartAsync(CancellationToken ct = default)
	{
		if (_options.IsEnabled is false) return;
		BotClient.StartReceiving(_baseUpdateHandler.UpdateHandler,
			PollingErrorHandler,
			cancellationToken: ct
		);

		// setting global tg-bot-user context for access from handlers
		BotIdentityCache.Instance.SetIdentity(await BotClient.GetMeAsync(ct));

		Log.Information("Bot {Name} start receiving", BotIdentityCache.Instance.BotUser!.Username);
	}

	public async Task StopAsync(CancellationToken ct = default)
	{
		if (_options.IsEnabled is false) return;
		await BotClient.CloseAsync(ct);
	}

	// all contol for exceptions will be outside
	private Task PollingErrorHandler(ITelegramBotClient client, Exception ex, CancellationToken ct = default)
	{
		Log.Error(ex, "Telegram Bot Polling error");
		return Task.CompletedTask;
	}

	~BotService()
	{
		if (_options.IsEnabled is false) return;
		BotClient.CloseAsync().Wait();
	}

	#endregion
}