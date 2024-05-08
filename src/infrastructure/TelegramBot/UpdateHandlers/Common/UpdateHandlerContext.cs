using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.UpdateHandlers.Common;

/// <summary>
/// Контекст входящего апдейта в одной модели
/// </summary>
public sealed class UpdateHandlerContext
{
	public UpdateHandlerContext() {
		
	}
	public UpdateHandlerContext(ITelegramBotClient client, Update update) {
		Client = client;
		Update = update;
		CancellationToken = new CancellationToken();
	}
	public UpdateHandlerContext(ITelegramBotClient client, Update update, CancellationToken cancellationToken) {
		Client = client;
		Update = update;
		CancellationToken = cancellationToken;
	}
	public ITelegramBotClient Client { get; set; }
	public Update Update { get; set; }
	public CancellationToken CancellationToken { get; set; }
}