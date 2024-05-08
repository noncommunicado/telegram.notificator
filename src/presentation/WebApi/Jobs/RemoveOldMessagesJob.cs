using Application.CQRS.Commands.Messages;
using MediatR;
using Quartz;
using Serilog;

namespace WebApi.Jobs;

public sealed class RemoveOldMessagesJob : IJob
{
	private readonly IMediator _mediator;
	public RemoveOldMessagesJob(IMediator mediator) {
		_mediator = mediator;
	}
	public async Task Execute(IJobExecutionContext context) {
		try {
			await _mediator.Send(new DeleteOldMessagesCommand());
		}
		catch (Exception e) {
			Log.Error(e, nameof(RemoveOldMessagesJob));
		}
	}
}