using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Interfaces;
using WebApi.Services;

namespace WebApi.Web.Endpoints.Auth.Login;

public sealed record Request(string Login, string Password);
public sealed record Response(bool Success, string Token = "", string? Error = null);

public sealed class LoginEndpoint : Endpoint<Request, Response>
{
	public IKutCodeLdapRepository LdapRepository { get; set; }
	public JwtTokenGenerator TokenGenerator { get; set; }
	public override void Configure()
	{
		Post("auth/ldap/login");
		AllowAnonymous();
		Version(1);
		Summary(s =>
		{
			s.Summary = "Авторизация через домен";
			s.Description = "Получить токен авторизации на основе групповых политик домена";
		});
	}

	public override async Task<Response> ExecuteAsync(Request req, CancellationToken ct)
	{
		LdapAuthenticationResponse ldapAuthResult = default;
		try
		{
			ldapAuthResult = await LdapRepository.LoginAsync(new(req.Login, req.Password), ct);
		}
		catch (Exception e)
		{
			HttpContext.Response.StatusCode = 401;
			return new Response(false, Error: e.Message);
		}
		
		if (ldapAuthResult!.Authorized == false)
		{
			HttpContext.Response.StatusCode = 401;
			return new Response(false, Error: "Вы ввели неправильное имя пользователя или пароль");
		}

		var token = TokenGenerator.GenerateToken(ldapAuthResult.UserData!);
		return new Response(true, token);
	}

}