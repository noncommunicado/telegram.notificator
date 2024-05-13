using Telegram.Bot.Types;

namespace TelegramBot.Cache;

/// <summary>
///     Singletone
/// </summary>
public class BotIdentityCache
{
	private static BotIdentityCache? I;
	private static readonly object Locker = new();

	private BotIdentityCache() { }

	public User? BotUser { get; private set; }

	public static BotIdentityCache Instance {
		get {
			lock (Locker) {
				return I ??= new BotIdentityCache();
			}
		}
	}

	public void SetIdentity(User user)
	{
		BotUser = user;
	}
}