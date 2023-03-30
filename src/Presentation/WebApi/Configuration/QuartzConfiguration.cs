
using Quartz;
using WebApi.Jobs;

namespace WebApi.Configuration;

public static class QuartzConfiguration
{
	internal static IServiceCollection ConfigureQuartz(this IServiceCollection services)
	{
		services.AddQuartz(q =>
		{
			q.UseMicrosoftDependencyInjectionJobFactory();
			var rmMsgJobKey = new JobKey(nameof(RemoveOldMessagesJob));
			q.AddJob<RemoveOldMessagesJob>(opts => opts.WithIdentity(rmMsgJobKey));
			q.AddTrigger(opt => opt.ForJob(rmMsgJobKey)
				.WithIdentity(nameof(RemoveOldMessagesJob)).WithSimpleSchedule(b => b.WithIntervalInMinutes(60))
			);
		});
		services.AddQuartzHostedService(options => {
			options.WaitForJobsToComplete = true;
		});
		return services;
	}
}