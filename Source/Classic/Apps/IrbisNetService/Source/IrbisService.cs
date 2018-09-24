// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisService.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ServiceProcess;

using AM.Logging;

using JetBrains.Annotations;

#endregion

namespace IrbisNetService
{
    /// <summary>
    /// Обертка над сервисом Windows.
    /// </summary>
    [PublicAPI]
    public class IrbisService
        : ServiceBase
    {
        #region Constants

        /// <summary>
        /// Service name.
        /// </summary>
        public const string IrbisNet = "IRBIS.NET";

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisService()
        {
            ServiceName = IrbisNet;
        }

        #endregion

        #region ServiceBase members

        /// <inheritdoc cref="ServiceBase.OnContinue" />
        protected override void OnContinue()
        {
            Log.Trace("IrbisService::OnContinue: enter");

            try
            {
                base.OnContinue();
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisService::OnContinue", exception);
            }

            Log.Trace("IrbisService::OnContinue: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnPause" />
        protected override void OnPause()
        {
            Log.Trace("IrbisService::OnPause: enter");

            try
            {
                base.OnPause();
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisService::OnPause", exception);
            }

            Log.Trace("IrbisService::OnPause: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnShutdown" />
        protected override void OnShutdown()
        {
            Log.Trace("IrbisService::OnShutdown: enter");

            try
            {
                base.OnShutdown();
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisService::OnShutdown", exception);
            }

            Log.Trace("IrbisService::OnShutdown: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnStart" />
        protected override void OnStart
            (
                string[] args
            )
        {
            Log.Trace("IrbisService::OnStart: enter");

            try
            {
                base.OnStart(args);
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisService::OnStart", exception);
            }

            Log.Trace("IrbisService::OnStart: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnStop" />
        protected override void OnStop()
        {
            Log.Trace("IrbisService::OnStop: enter");

            try
            {
                base.OnStop();
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisService::OnStop", exception);
            }

            Log.Trace("IrbisService::OnStop: leave");
        }

        #endregion
    }
}
