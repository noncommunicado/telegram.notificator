using Application.Interfaces;
using Domain.BotService;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot;

public sealed class BotService : IBotHostedService
{
	private readonly TelegramBotClient _botClient; 
	public BotService(TelegramBotOptions options)
	{
		_botClient = new TelegramBotClient(options.Token);
	}

	// all contol for exceptions will be outside
	private Task PollingErrorHandler(ITelegramBotClient client, Exception ex, CancellationToken ct)
	{
		return Task.CompletedTask;
	}

	private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken ct)
	{
		if (update.Type == UpdateType.Message) {
			try {
				await client.SendTextMessageAsync(update.Message!.Chat.Id, $"Ваш ID:{Environment.NewLine}<pre>{update.Message.Chat.Id}</pre>", ParseMode.Html, cancellationToken: ct).ConfigureAwait(false);
			}
			catch (Exception e) {
				Log.Error(e, "Can't send chat-id response");
			}
		}
	}

	public async Task SendAsync(SendMessageModel request, CancellationToken ct)
		=> await _botClient.SendTextMessageAsync(
				request.ChatId,
				request.Message.Text,
				ParseMode.Html,
				disableNotification: request.Message.DisableNotification, 
				cancellationToken: ct)
			.ConfigureAwait(false);

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_botClient.StartReceiving(updateHandler: UpdateHandler, pollingErrorHandler: PollingErrorHandler, cancellationToken: cancellationToken);
		return Task.CompletedTask;
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		 await _botClient.CloseAsync(cancellationToken: cancellationToken);
	}
	~BotService()
	{
		_botClient.CloseAsync().Wait();
	}
}