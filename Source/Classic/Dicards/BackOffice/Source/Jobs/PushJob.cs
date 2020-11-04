// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedType.Global

/* PushJob.cs -- задание, периодически запускающее пуш-уведомления.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System.Threading.Tasks;

using Topshelf.Logging;

using Quartz;

#endregion

namespace BackOffice.Jobs
{
    /// <summary>
    /// Задание, периодически запускающее пуш-уведомления.
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class PushJob
        : IJob
    {
        #region Private members

        private static readonly LogWriter _log = HostLogger.Get<PushJob>();

        #endregion

        #region Public members

        /// <summary>
        /// Метод вызывается планировщиком.
        /// </summary>
        public async Task Execute
            (
                IJobExecutionContext context
            )
        {
            await Task.Run(Pusher.DoWork);
        }

        #endregion
    }
}