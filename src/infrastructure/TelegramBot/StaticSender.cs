using Serilog;
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
			GetMessagePrivateText(context),
			parseMode: ParseMode.Html,
			cancellationToken: context.CancellationToken);
	}

	public static Task SendGroupIdAsync(UpdateHandlerContext context)
	{
		Log.Information("Sending reply chat Id to {ChatId};", context.Update.Message?.Chat.Id);
		return context.Client.SendTextMessageAsync(
			context.Update.Message!.Chat.Id,
			GetMessageGroupText(context),
			messageThreadId: context.Update.Message.MessageThreadId,
			parseMode: ParseMode.Html,
			cancellationToken: context.CancellationToken);
	}

	private static string GetMessagePrivateText(UpdateHandlerContext context)
	{
		return
			$"🔶 Id чата: <pre>{context.Update.Message.Chat.Id}</pre>{Environment.NewLine}<i>Сообщите его админу для добавления чата</i>";
	}
	private static string GetMessageGroupText(UpdateHandlerContext context)
	{
		if (context.Update.Message.MessageThreadId is null)
			return
				$"🔶 Id группы: <pre>{context.Update.Message.Chat.Id}</pre>{Environment.NewLine}<i>Сообщите его админу для добавления группы</i>";
		else
			return
				$"🔶 Id группы: <pre>{context.Update.Message.Chat.Id}</pre>{Environment.NewLine}⚠️ Id топика: <pre>{context.Update.Message.MessageThreadId}</pre>{Environment.NewLine}<i>Сообщите его админу для добавления топика</i>";
	}
}