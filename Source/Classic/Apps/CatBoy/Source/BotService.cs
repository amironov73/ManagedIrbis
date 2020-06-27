// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisService.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;

using AM.Logging;

using JetBrains.Annotations;

#endregion

// ReSharper disable CommentTypo

namespace CatBoy
{
    /// <summary>
    /// Обертка над сервисом Windows.
    /// </summary>
    [PublicAPI]
    public class BotService
        : ServiceBase
    {
        #region Constants

        /// <summary>
        /// Service name.
        /// </summary>
        public const string IrbisBot = "CAT.BOY";

        #endregion

        #region Properties

        [CanBeNull]
        public Task WorkTask { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BotService()
        {
            ServiceName = IrbisBot;
            AutoLog = true;
        }

        #endregion

        #region ServiceBase members

        /// <inheritdoc cref="ServiceBase.OnContinue" />
        protected override void OnContinue()
        {
            Log.Trace("BotService::OnContinue: enter");

            try
            {
                base.OnContinue();

                Bot.Paused = false;
            }
            catch (Exception exception)
            {
                Log.TraceException("BotService::OnContinue", exception);
            }

            Log.Trace("BotService::OnContinue: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnPause" />
        protected override void OnPause()
        {
            Log.Trace("BotService::OnPause: enter");

            try
            {
                base.OnPause();

                Bot.Paused = true;
            }
            catch (Exception exception)
            {
                Log.TraceException("BotService::OnPause", exception);
            }

            Log.Trace("BotService::OnPause: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnShutdown" />
        protected override void OnShutdown()
        {
            Log.Trace("BotService::OnShutdown: enter");

            try
            {
                var client = Bot.GetClient();
                client.StopReceiving();

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

                string message = "Arguments: " + string.Join(" ", args);
                EventLog.WriteEntry(message, EventLogEntryType.Information);

                Bot.GetClient();
                Bot.MessageLoop();
            }
            catch (Exception exception)
            {
                Log.TraceException("BotService::OnStart", exception);
            }

            Log.Trace("BotService::OnStart: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnStop" />
        protected override void OnStop()
        {
            Log.Trace("BotService::OnStop: enter");

            try
            {
                var client = Bot.GetClient();
                client.StopReceiving();

                base.OnStop();
            }
            catch (Exception exception)
            {
                Log.TraceException("BotService::OnStop", exception);
            }

            Log.Trace("BotService::OnStop: leave");
        }

        #endregion
    }
}
