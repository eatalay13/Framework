using Hangfire;
using BackgroundJobs.Managers.RecurringJobs;
using System;

namespace BackgroundJobs.Schedules
{
    /// <summary>
    /// Çok kez (tekrarlı işler) ve belirtilmiş CRON süresince çalışır
    /// </summary>
    public static class RecurringJobs
    {
        public static void DailyTasksCheck()
        {
            RecurringJob.RemoveIfExists(nameof(DailyTaskReNotCompleteManager));
            RecurringJob.AddOrUpdate<DailyTaskReNotCompleteManager>(nameof(DailyTaskReNotCompleteManager),
                job => job.Process(), "45 23 * * *", TimeZoneInfo.Local);
        }

        public static void SessionRequest()
        {
            RecurringJob.RemoveIfExists(nameof(WakeUpManager));
            RecurringJob.AddOrUpdate<WakeUpManager>(nameof(WakeUpManager),
                job => job.Process(), "*/3 * * * *", TimeZoneInfo.Local);
        }
    }
}