// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* JobController.cs -- управление заданиями
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using AM;
using Quartz;
using Quartz.Impl;

using Topshelf.Logging;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace BackOffice.Jobs
{
    /// <summary>
    /// Управление заданиями.
    /// </summary>
    internal sealed class JobController
    {
        #region Private members

        private static IScheduler _scheduler;
        private static readonly LogWriter _log = HostLogger.Get<JobController>();
        
        /// <summary>
        /// Создание задания и добавление его в планировщик.
        /// </summary>
        private static void CreateAndScheduleJob
            (
                JobDescription description
            )
        {
            var jobType = Type.GetType(description.Type, true);

            var job = JobBuilder.Create()
                .OfType(jobType)
                .WithIdentity(description.Name, description.Group)
                .WithDescription(description.Description)
                .Build();
            
            var trigger = TriggerBuilder.Create()
                .WithIdentity(description.Name, description.Group)
                .WithDescription(description.Description)
                .WithCronSchedule(description.CronExpression)
                .Build();
             
            _scheduler.ScheduleJob(job, trigger)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            
            _log.InfoFormat
                (
                    "Job {0} created with cron \"{1}\"", 
                    description.Name,
                    description.CronExpression
                );
        }

        /// <summary>
        /// Прямой запуск задания (минуя планировщик)
        /// в синхронном режиме (дожидается окончания).
        /// </summary>
        private static void DirectRun
            (
                JobDescription description
            )
        {
            var jobType = Type.GetType(description.Type, true);
            var jobObject = Activator.CreateInstance(jobType);
            var method = jobType.GetMethod("Execute");
            if (ReferenceEquals(method, null))
            {
                throw new InvalidOperationException("Method 'Execute' not found");
            }

            var task = (Task) method.Invoke
                (
                    jobObject,
                    new object[] { null }
                );
                
            task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Загрузка разрешенных заданий из JSON-файла.
        /// </summary>
        private static JobDescription[] LoadJobs()
        {
            var jobFileName = CM.AppSettings["jobs"];
            var result = JobDescription
                .ReadJobs(jobFileName)
                .Where(job => job.Enabled)
                .ToArray();

            _log.InfoFormat("{0} job(s) loaded", result.Length);

            if (result.Length == 0)
            {
                throw new ApplicationException("No enabled jobs found");
            }
            
            return result;
        }
        
        #endregion
        
        #region Public methods

        /// <summary>
        /// Загрузка всех разрешенных заданий и передача их в планировщик.
        /// </summary>
        public static async Task ScheduleJobs()
        {
            var factory = new StdSchedulerFactory();
            _scheduler = await factory.GetScheduler();
            await _scheduler.Start();

            var jobs = LoadJobs();
            
            foreach (var job in jobs)
            {
                CreateAndScheduleJob(job);
            }
        }

        /// <summary>
        /// Загрузка всех разрешенных заданий и выполнение их вручную.
        /// Либо выполнение одного указанного задания.
        /// </summary>
        public static void RunJobsManually
            (
                string jobName
            )
        {
            _log.Info(nameof(RunJobsManually) + ": begin");
            
            var jobs = LoadJobs();
            var found = false;
            
            foreach (var job in jobs)
            {
                if (string.IsNullOrEmpty(jobName))
                {
                    found = true;
                    DirectRun(job);
                }
                else
                {
                    if (jobName.SameString(job.Name))
                    {
                        found = true;
                        DirectRun(job);
                    }
                }
            }

            if (!found)
            {
                _log.Error("No suitable job found");
            }
            
            _log.Info(nameof(RunJobsManually) + ": end");
        }

        /// <summary>
        /// Штатная остановка планировщика.
        /// </summary>
        public static async Task StopScheduler()
        {
            _log.Info("Start: " + nameof(StopScheduler));
            
            await _scheduler.Shutdown();
            
            _log.Info("Success: " + nameof(StopScheduler));
        }
        
        #endregion
    }
}
