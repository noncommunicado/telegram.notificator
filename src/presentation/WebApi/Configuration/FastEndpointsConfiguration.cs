using System.Globalization;
using AspNetCore.Authentication.ApiKey;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.Net.Http.Headers;
using NSwag;
using WebApi.Services;
using WebApi.Settings;
using WebApi.Web;

namespace WebApi.Configuration;

public static class FastEndpointsConfiguration
{
	public static WebApplicationBuilder AddFastEndpointsConfiguration(this WebApplicationBuilder builder)
	{
		var securitySettings = builder.Configuration
			.GetSection("Authorization")
			.Get<AuthorizationSettings>();
		builder.Services
			.AddOptions<AuthorizationSettings>()
			.Configure(model => builder.Configuration.Bind("Authorization", model));

		builder.Services.AddSingleton<JwtTokenGenerator>();

		builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
		builder.Services.AddFastEndpoints(o => {
			o.IncludeAbstractValidators = true;
		});
		builder.Services.SwaggerDocument(x => {
			x.MaxEndpointVersion = 1;
			x.ShortSchemaNames = true;
			x.RemoveEmptyRequestSchema = true;
			x.EnableJWTBearerAuth = false;
			x.DocumentSettings = s => {
				s.DocumentName = "v1";
				s.Description = SwaggerDescription;
				s.AddAuth(AuthenticationSchemes.StaticApiKey, new OpenApiSecurityScheme {
					Name = CustomHeaderNames.StaticApiKey,
					In = OpenApiSecurityApiKeyLocation.Header,
					Type = OpenApiSecuritySchemeType.ApiKey,
					Description = "Использовать эту авторизацию для приложений со статическим токеном"
				});
				s.AddAuth(AuthenticationSchemes.JwtScheme, new OpenApiSecurityScheme {
					Name = HeaderNames.Authorization,
					In = OpenApiSecurityApiKeyLocation.Header,
					Type = OpenApiSecuritySchemeType.ApiKey,
					Description = "Использовать эту авторизацию для JWT Bearer"
				});
			};
			x.TagDescriptions = t =>
			{
				t["default"] = "Client generators";
				t["Attachments"] = "Upload attachments";
				t["Auth"] = "Authentication methods";
				t["Group"] = "Manipualitions with send-groups";
				t["Telegram"] = "Send to telegram methods";
				t["Forum"] = "Forum interaction methods";
			};
		});

		ConfigureAuthorization(builder, securitySettings);
		// Set fluent validation error language to russian
		ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");
		return builder;
	}

	private static void ConfigureAuthorization(WebApplicationBuilder builder, AuthorizationSettings? settings)
	{
		if (settings is null) return;
		builder.Services.AddAuthorization();

		builder.Services.AddAuthenticationJwtBearer(options => { options.SigningKey = settings.Jwt.SignInKey; });
		builder.Services.AddAuthentication()
			.AddApiKeyInHeader(AuthenticationSchemes.StaticApiKey, opts => {
					opts.IgnoreAuthenticationIfAllowAnonymous = true;
					opts.Realm = AuthenticationSchemes.StaticApiKey;
					opts.KeyName = CustomHeaderNames.StaticApiKey;
					opts.Events.OnValidateKey = ctx => {
						if (ctx.ApiKey.Equals(settings.Static.Key, StringComparison.InvariantCulture))
							ctx.ValidationSucceeded();
						else
							ctx.ValidationFailed();
						return Task.CompletedTask;
					};
				}
			);
	}

	private const string SwaggerDescription = @"
Данный сервис поддерживает отправку текстовых сообщений, документов и изображений, с группировкой и без, в `HTML` формате, в **тихом** режиме или без него *(без уведомления)*.

А также отправку всего этого в группы и каналы групп *(Threads - Темы)*.

Для вызова API вы можете написать собственное решение или скачать готовый авто-генерируемый репозиторий в группе методов `default` прямо тут, из *Swagger*.

____
В группе методов `Group` вы можете предсоздать группы чатов для последующего использования в методах отправки сообщения.   
В таких группах может быть много чатов, причем, разных типов, как личные чаты, так и группы.   
И Всем участникам таких ""вирутальных"" групп придет одно и то же сообщение.  
____
Вы можете загружать и переиспользовать вложения, с.м. методы `Attachments`.  
После загрузки вложения вам вернется его идентификатор в системе, сохраните его и используйте в методах отправки сообщений.  
Сервис автоматически определит тип вложения. Если это изображения и включена группировка, то сервис отправит изображения группой.  
Документы (не изображения) будут отправлены как файлы и доступны для скачивания.
";
}