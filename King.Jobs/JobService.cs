using Hangfire;
using log4net;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace King.Jobs
{
    /// <summary>
    /// Hangfire 任务
    /// </summary>
    public class JobService : BackgroundService
    {
        public readonly ILog log = LogManager.GetLogger("King", typeof(JobService));
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IRecurringJobManager _recurringJobs;
        public JobService(
          IBackgroundJobClient backgroundJobs,
          IRecurringJobManager recurringJobs
        )
        {
            _backgroundJobs = backgroundJobs;
            _recurringJobs = recurringJobs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                //RecurringJob.AddOrUpdate(() => new JobService().StartAsync(), Cron.Minutely());
                _recurringJobs.AddOrUpdate<DemoTask>("seconds", i => i.Run(), "*/20 * * * * ?", queue: "default");

                _recurringJobs.AddOrUpdate<TestTask>("min", i => i.Run(), Cron.Minutely(), queue: "default");
            }
            catch (Exception e)
            {
                log.Error("An exception occurred while creating recurring jobs.", e);
            }

            return Task.CompletedTask;
        }
    }
}
