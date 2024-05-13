using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.UpdateHandlers.Common;

namespace TelegramBot;

public sealed class StaticSender
{
	public static Task SendUserIdAsync(UpdateHandlerContext context)
	{
		return context.Client.SendTextMessageAsync(
			context.Update.Message!.Chat.Id,
			$"Id чата: <pre>{context.Update.Message.Chat.Id}</pre>{Environment.NewLine}{Environment.NewLine}<i>Сообщите его @hamaronooo</i>",
			parseMode: ParseMode.Html,
			cancellationToken: context.CancellationToken);
	}

	public static Task SendGroupIdAsync(UpdateHandlerContext context)
	{
		return context.Client.SendTextMessageAsync(
			context.Update.Message!.Chat.Id,
			$"Id группы: <pre>{context.Update.Message.Chat.Id}</pre>{Environment.NewLine}{Environment.NewLine}<i>Сообщите его @hamaronooo</i>",
			parseMode: ParseMode.Html,
			cancellationToken: context.CancellationToken);
	}
}