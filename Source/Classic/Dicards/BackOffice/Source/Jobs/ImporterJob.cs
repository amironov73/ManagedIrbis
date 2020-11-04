﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedType.Global

/* ImporterJob.cs -- задание, периодически запускающее импорт читателей.
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
    /// Задание, периодически запускающее импорт читателей
    /// из репозитория DICARDS.
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class ImporterJob 
        : IJob
    {
        #region Private members
        
        private static readonly LogWriter _log = HostLogger.Get<ImporterJob> ();
        
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
            await Task.Run(Importer.DoWork);
        }
        
        #endregion
    }
}
