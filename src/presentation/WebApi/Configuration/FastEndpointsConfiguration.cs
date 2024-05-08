using System.Globalization;
using AspNetCore.Authentication.ApiKey;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
		
		builder.Services.AddFastEndpoints(o => {
			o.IncludeAbstractValidators = true;
		});
		builder.Services.SwaggerDocument(x => {
			x.MaxEndpointVersion = 1;
			x.ShortSchemaNames = true;
			x.RemoveEmptyRequestSchema = true;
			x.EnableJWTBearerAuth = true;
			x.DocumentSettings = s => {
				s.AddAuth(AuthSchemes.StaticApiKey, new()
				{
					Name = CustomHeaderNames.StaticApiKey,
					In = OpenApiSecurityApiKeyLocation.Header,
					Type = OpenApiSecuritySchemeType.ApiKey,
				});
				s.AddAuth(JwtBearerDefaults.AuthenticationScheme, new() {
					Name = HeaderNames.Authorization,
					In = OpenApiSecurityApiKeyLocation.Header,
					Type = OpenApiSecuritySchemeType.ApiKey,
				});
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
		
		if (settings.Enabled) {
			builder.Services.AddAuthenticationJwtBearer(options => {
				options.SigningKey = settings.Jwt.SignInKey;
			});
			builder.Services.AddAuthentication()
				.AddApiKeyInHeader(AuthSchemes.StaticApiKey, opts => {
						opts.IgnoreAuthenticationIfAllowAnonymous = true;
						opts.Realm = AuthSchemes.StaticApiKey;
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
	}
}