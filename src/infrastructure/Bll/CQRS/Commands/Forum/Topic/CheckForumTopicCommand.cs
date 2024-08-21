using Domain.Interfaces;
using MediatR;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace Bll.CQRS.Commands.Forum.Topic;

public sealed record CheckForumTopicCommandResult(bool ChatExist, bool TopicExist = false);
public sealed record CheckForumTopicCommand(long ChatId, int ThreadId) : IRequest<CheckForumTopicCommandResult>;
public sealed class CheckForumTopicCommandRequestHandler : IRequestHandler<CheckForumTopicCommand, CheckForumTopicCommandResult>
{
	private readonly IBotHostedService _botService;
	public CheckForumTopicCommandRequestHandler(IBotHostedService botService) => _botService = botService;
	public async Task<CheckForumTopicCommandResult> Handle(CheckForumTopicCommand request, CancellationToken ct)
	{
		// ⚡️ 5312016608254762256
		// ⭐️ 5235579393115438657
		// exists: Bad Request: TOPIC_NOT_MODIFIED
		// not exists: Bad Request: TOPIC_ID_INVALID
		CheckForumTopicCommandResult? result = null;
		try {
			await _botService.BotClient.ReopenForumTopicAsync(request.ChatId, request.ThreadId, ct);
		}
		catch (ApiRequestException apiEx) when (apiEx.Message.ToLower().Contains("topic_not_modified")) {
			result = new CheckForumTopicCommandResult(true, true);
		}
		catch (ApiRequestException apiEx) when (apiEx.Message.ToLower().Contains("topic_id_invalid")) {
			result = new CheckForumTopicCommandResult(true);
		}
		catch (ApiRequestException apiEx) when (apiEx.Message.ToLower().Contains("chat not found")) {
			result = new CheckForumTopicCommandResult(false);
		}
		catch {
			throw;
		}

		if (result is null) {
			try
			{
				await _botService.BotClient.CloseForumTopicAsync(request.ChatId, request.ThreadId, ct);
			}
			catch (Exception e)
			{
				//swallow
				Log.Error(e, "Failed to close forum topic after reopening (in check purpose); Request: {CheckForumTopicCommand}", request);
			}
			return new CheckForumTopicCommandResult(true, true);
		}

		Log.Information("Check forum topic command result returns: {ForumTopicCheckData} for topic {CheckForumTopicCommand}", result, request);
		return result;
	}
}