using Hangfire;
using System;

namespace BackgroundJobs.Schedules
{
    public static class ContinuationJobs
    {
        /// <summary>
        /// Birbiri ile ilişkili işlerin olduğu zaman çalışan job. Job tetiklenmesi için başka bir job bitmesi gerekiyor
        /// </summary>
        /// <param name="id">İlişkili job id değeri</param>

        // farklı işler yapan metotları burada tanımlayabiliriz. tabi  ContinuationJobs  türünde çalışan
    }
}


