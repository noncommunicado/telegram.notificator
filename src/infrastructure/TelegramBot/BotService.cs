using Application.Interfaces;
using Domain.BotService;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.Cache;
using TelegramBot.UpdateHandlers;

namespace TelegramBot;

public sealed class BotService : IBotHostedService
{
	private readonly BaseUpdateHandler _baseUpdateHandler = new();
	private readonly TelegramBotClient _botClient;

	public BotService(TelegramBotOptions options)
	{
		_botClient = new TelegramBotClient(options.Token);
	}

	public async Task SendAsync(SendMessageModel request, CancellationToken ct)
	{
		await _botClient.SendTextMessageAsync(
				request.ChatId,
				request.Message.Text,
				parseMode: ParseMode.Html,
				disableNotification: request.Message.DisableNotification,
				cancellationToken: ct)
			.ConfigureAwait(false);
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		_botClient.StartReceiving(_baseUpdateHandler.UpdateHandler,
			PollingErrorHandler,
			cancellationToken: cancellationToken
		);

		// setting global tg-bot-user context for access from handlers
		BotIdentityCache.Instance.SetIdentity(await _botClient.GetMeAsync(cancellationToken));

		Log.Information("Bot {Name} start receiving", BotIdentityCache.Instance.BotUser!.Username);
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await _botClient.CloseAsync(cancellationToken);
	}

	// all contol for exceptions will be outside
	private Task PollingErrorHandler(ITelegramBotClient client, Exception ex, CancellationToken ct)
	{
		return Task.CompletedTask;
	}

	~BotService()
	{
		_botClient.CloseAsync().Wait();
	}
}