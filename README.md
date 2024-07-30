## Сервис отправки уведомлений через Telegram бота

## ⚠️⚠️⚠️  
Для начала разработки добавьте источник пакетов `NuGet`
```shell
dotnet nuget add source https://nuget.voids.site/v3/index.json
dotnet add package Telegram.Bot
```
Без добавления этого источника, максимальная версия пакета `Telegram.Bot` будет v19, они перестали публиковать пакет NuGet Microsoft.  

----

Данный сервис поддерживает отправку текстовых сообщений, документов и изображений, с группировкой и без, в `HTML` формате, в "тихом" режиме или без него (без уведомления).  

А также отправку всего этого в группы и каналы групп (Threads - Темы).

Для вызова API вы можете написать собственное решение или скачать готовый авто-генерируемый репозиторий в группе методов `default` прямо из swagger.

____

В группе методов `Group` вы можете предсоздать группы чатов для последующего использования в методах отправки сообщения.   
В таких группах может быть много чатов, причем, разных типов, как личные чаты, так и группы.   
И Всем участникам таких "вирутальных" групп придет одно и то же сообщение.  


____

Вы можете загружать и переиспользовать вложения, с.м. методы `Attachments`.  
После загрузки вложения вам вернется его идентификатор в системе, сохраните его и используйте в методах отправки сообщений.  
Сервис автоматически определит тип вложения. Если это изображения и включена группировка, то сервис отправит изображения группой.  
Документы (не изображения) будут отправлены как файлы и доступны для скачивания.