using Hangfire;
using log4net;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace King.Jobs
{
    /// <summary>
    /// Hangfire 任务
    /// </summary>
    public class JobService
    {
        public readonly ILog log = LogManager.GetLogger("King", typeof(JobService));
        public async Task StartAsync()
        {
            await Task.Run(() =>
            {

                Console.WriteLine("执行任务了......StartAsync");
                log.Info("JobService执行任务");
            });

            ////hangfire定时任务
            //var id = Hangfire.BackgroundJob.Enqueue(() => Console.WriteLine("插入队列的任务"));
            //Hangfire.BackgroundJob.Schedule(() => Console.WriteLine("延迟的任务"), TimeSpan.FromSeconds(5));
            //Hangfire.RecurringJob.AddOrUpdate(() => Console.WriteLine("循环执行的任务"), Hangfire.Cron.Minutely);
            //Hangfire.BackgroundJob.ContinueJobWith(id, () => Console.WriteLine("指定任务执行之后执行的任务"));
        }
    }
}
