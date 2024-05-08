
using Telegram.Bot.Types;

namespace TelegramBot.Cache;

/// <summary>
/// Singletone
/// </summary>
public class BotIdentityCache
{
	private static BotIdentityCache? I = null;
	private static object Locker = new();
	public User? BotUser { get; private set; }
	
	public static BotIdentityCache Instance {
		get {
			lock (Locker) {
				return I ??= new BotIdentityCache();
			}
		}
	}
	private BotIdentityCache() {
		
	}

	public void SetIdentity(User user) {
		BotUser = user;
	}
}