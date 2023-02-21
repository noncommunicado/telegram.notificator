## Сервис уведомлений через Telegram бота
* <a href="установка">Установка</a>
* <a href="использование">Использование</a>

### Установка

1. Добавьте новый NuGet Source (источник пакетов) со следующим URL: http://nuget.neftm.local/nuget
2. Установите в нужный проект пакет `Neftm.Telegram.Notificator` через `NuGet`
3. Запустите NuGet Restore (восстановление пакетов)

### Использование

``` csharp
// add to DI
services.AddTelegramNotificator();

// Use
var repo = services.GetService<INotificatorRepository>();
await repo.SendToChatsAsync(new SendToChatsRequest("Test sending", false, new[] {(long) 12345678}), new CancellationToken());
await repo.SendToGroupsAsync(new SendToChatsRequest("Test sending", false, new[] { "test-group" }), new CancellationToken());
```