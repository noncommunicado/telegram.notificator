using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Cache;
using TelegramBot.UpdateHandlers.Common;

namespace TelegramBot.UpdateHandlers;

public sealed class BaseUpdateHandler
{
	public async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken ct) {
		UpdateHandlerContext context = new(client, update, ct);
		
		if (update is {Type: UpdateType.Message, Message: { }}) {
			if (update.Message.Chat is {Type: ChatType.Private})
				await HandlePrivateChatAsync(context).ConfigureAwait(false);
			if (update.Message.Chat is { Type: ChatType.Group or ChatType.Supergroup })
				await HandleGroupMessageAsync(context).ConfigureAwait(false);
		}
	}

	private Task HandlePrivateChatAsync(UpdateHandlerContext context) {
		return StaticSender.SendUserIdAsync(context);
	}

	private Task HandleGroupMessageAsync(UpdateHandlerContext context) {
		if (IsMentionOfMe(context))
			return StaticSender.SendGroupIdAsync(context);

		return Task.CompletedTask;
	}

	/// <summary>
	/// Проверка, содержится ли в сообщение упоминание бота (через @)
	/// </summary>
	private static bool IsMentionOfMe(UpdateHandlerContext context) {
		if (context.Update.Message!.Entities == null || context.Update.Message.EntityValues == null)
			return false;
		
		var mentionIds = context.Update.Message!.Entities
			.Select((obj, id) => (obj, id))
			.Where(tuple => tuple.obj.Type == MessageEntityType.Mention)
			.Select(x => x.id).ToArray();

		if (mentionIds.Any() == false)
			return false;
		
		foreach (var mentionId in mentionIds) {
			var mentionValue = string.Intern(context.Update.Message.EntityValues?.ToArray()[mentionId].TrimStart('@') ?? string.Empty);
			if (mentionValue == BotIdentityCache.Instance.BotUser!.Username)
				return true;
		}

		return false;
	}
}