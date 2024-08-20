using Humanizer;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Cache;
using TelegramBot.UpdateHandlers.Common;

namespace TelegramBot.UpdateHandlers;

public sealed class BaseUpdateHandler
{
	public async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken ct)
	{
		Log.Information("Bot. Id: {UpdateId}; Type: {UpdateType}", update.Id, update.Type);
		if (update.Type == UpdateType.Message && update.Message is not null)
			Log.Information("Bot. Id: {UpdateId}; ChatId: {ChatId}; Text: {UpdateText}", 
				update.Id, update.Message.Chat.Id, update.Message.Text.Truncate(50));
		
		UpdateHandlerContext context = new(client, update, ct);

		if (update is {Type: UpdateType.Message, Message: not null}) {
			if (update.Message.Chat is {Type: ChatType.Private})
				await HandlePrivateChatAsync(context).ConfigureAwait(false);
			if (update.Message.Chat is {Type: ChatType.Group or ChatType.Supergroup or ChatType.Channel})
				await HandleGroupMessageAsync(context).ConfigureAwait(false);
		}
	}
	private async Task HandlePrivateChatAsync(UpdateHandlerContext context)
	{
		if (await HandleCommandsAsync(context)) return;
		await StaticSender.SendUserIdAsync(context);
	}
	private async  Task HandleGroupMessageAsync(UpdateHandlerContext context)
	{
		if (await HandleCommandsAsync(context)) return;
		if (IsMentionOfMe(context))
			await  StaticSender.SendGroupIdAsync(context);
	}
	
	private async Task<bool> HandleCommandsAsync(UpdateHandlerContext context)
	{
		if (false == context.Update.Message?.Text?.StartsWith("/"))
			return false;
		// todo: finish command handling
		// switch (expression) {
		// 	
		// }
		// var emojies = context.Update.Message!
		// 	.Entities?.Where(x => x.Type == MessageEntityType.CustomEmoji).ToList();
		// if (emojies is null || emojies.Count == 0) {
		// 	await context.Client.SendTextMessageAsync(context.Update.Message!.Chat.Id, "",
		// 		cancellationToken: context.CancellationToken);
		// }
		// foreach (var command in emojies) {
		// 	
		// }

		return true;
	}


	/// <summary>
	///     Проверка, содержится ли в сообщение упоминание бота (через @)
	/// </summary>
	private static bool IsMentionOfMe(UpdateHandlerContext context)
	{
		if (context.Update.Message!.Entities == null || context.Update.Message.EntityValues == null)
			return false;

		var mentionIds = context.Update.Message!.Entities
			.Select((obj, id) => (obj, id))
			.Where(tuple => tuple.obj.Type == MessageEntityType.Mention)
			.Select(x => x.id).ToArray();

		if (mentionIds.Any() == false)
			return false;

		foreach (var mentionId in mentionIds) {
			var mentionValue = string.Intern(context.Update.Message.EntityValues?.ToArray()[mentionId].TrimStart('@') ??
			                                 string.Empty);
			if (mentionValue == BotIdentityCache.Instance.BotUser!.Username)
				return true;
		}

		return false;
	}
}