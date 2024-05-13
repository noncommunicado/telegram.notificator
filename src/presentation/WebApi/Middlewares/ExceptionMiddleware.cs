using Domain.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;

namespace WebApi.Middlewares;

public sealed class ExceptionMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try {
			await next(context);
		}
		catch (HttpRequestException re) {
			Log.Error(re, "HttpRequestException");
			context.Response.StatusCode = (int) re.StatusCode!;
			await context.Response.WriteAsJsonAsync(new ExceptionResponse(context.Response.StatusCode,
				re.Message));
		}
		catch (ValidationException re) {
			Log.Error(re, "ValidationException");
			context.Response.StatusCode = 400;

			await context.Response.WriteAsJsonAsync(
				new ExceptionResponse(context.Response.StatusCode,
					Errors: re.Errors.Select(x => new ExceptionResponseError(x.PropertyName, x.ErrorMessage)))
			);
		}
		catch (DbUpdateException ex) when (ex.InnerException is PostgresException {SqlState: "23505"}) {
			context.Response.StatusCode = 409;
			await context.Response.WriteAsJsonAsync(new ExceptionResponse(context.Response.StatusCode,
				"Такой объект уже существует"));
		}
		catch (DomainException ex) {
			Log.Error(ex, "Domain exception");
			context.Response.StatusCode = ex.HttpStatusCode;
			await context.Response.WriteAsJsonAsync(new ExceptionResponse(context.Response.StatusCode, ex.Message));
		}
		catch (Exception ex) {
			Log.Error(ex, "Unhandled exception");
			context.Response.StatusCode = 500;
			await context.Response.WriteAsJsonAsync(new ExceptionResponse(context.Response.StatusCode,
				"Unhandled server exception"));
		}
	}
}