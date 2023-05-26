using Hangfire;

namespace HangfireDemo.Services
{
    public class BackgroundJobService
    {
        public void RegisterRecurringJobs()
        {
            RecurringJob.AddOrUpdate("A Job Fire Every Minute", () => Console.WriteLine("This job execute automatically after a minute."), Cron.Minutely);
        }
    }
}
