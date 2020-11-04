// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* DummyService.cs -- сервис-заглушка, запускающий и останавливающий Quartz
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using BackOffice.Jobs;

using Topshelf;
using Topshelf.Logging;

#endregion

namespace BackOffice
{
    /// <summary>
    /// Сервис-заглушка, запускающий и останавливающий
    /// планировщик Quartz.
    /// </summary>
    public sealed class DicardsService 
        : ServiceControl
    {
        #region Private members
        
        private LogWriter _log;
        
        #endregion

        #region Public methods
        
        /// <summary>
        /// Запуск сервиса. 
        /// </summary>
        public bool Start
            (
                HostControl hostControl
            )
        {
            _log = HostLogger.Get<DicardsService>();
            _log.Info (nameof(DicardsService) + "::" + nameof (Start));

            Importer.LoadConfiguration();
            Pusher.LoadConfiguration();
            Reminder.LoadConfiguration();

            JobController.ScheduleJobs()
                .ConfigureAwait(false).GetAwaiter().GetResult();
            
            return true;
        }
        
        /// <summary>
        /// Остановка сервиса. 
        /// </summary>
        public bool Stop
            (
                HostControl hostControl
            )
        {
            _log.Info (nameof(DicardsService) + "::" + nameof (Stop));
            
            JobController.StopScheduler()
                .ConfigureAwait(false).GetAwaiter().GetResult();
            
            return true;
        }
        
        #endregion
    }
}
