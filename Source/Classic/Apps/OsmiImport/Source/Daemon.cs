// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Daemon.cs -- демон, занимается периодическим выполнением процедуры
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading;

using AM.Configuration;
using AM.Logging;

#endregion

// ReSharper disable LocalizableElement

namespace OsmiImport
{
    /// <summary>
    /// Демон. Занимается периодическим выполнением указанной процедуры.
    /// </summary>
    static class Daemon
    {
        #region Properties

        /// <summary>
        /// Признак временной паузы.
        /// </summary>
        public static bool Paused { get; set; }

        /// <summary>
        /// Шедулер запущен?
        /// </summary>
        public static bool Running { get; set; }

        /// <summary>
        /// Интервал между запусками рабочей процедуры,
        /// миллисекунды.
        /// </summary>
        public static int WorkingInterval { get; set; }

        /// <summary>
        /// Интервал между пробуждениями, миллисекунды.
        /// </summary>
        public static int SleepInterval { get; set; }

        /// <summary>
        /// Периодически выполняемое действие.
        /// </summary>
        public static Action WorkAction { get; set; }

        /// <summary>
        /// Рабочий поток.
        /// </summary>
        public static Thread WorkThread { get; set; }

        #endregion

        #region Private members

        private static void _Scheduler()
        {
            Log.Trace("Daemon::_Scheduler: enter");

            int remaining = WorkingInterval;
            int delay = SleepInterval;

            while (Running)
            {
                try
                {
                    Thread.Sleep(delay);
                    remaining -= delay;
                    if (remaining <= 0)
                    {
                        remaining = WorkingInterval;
                        if (!Paused)
                        {
                            WorkAction();
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.TraceException("Daemon::_Scheduler", exception);
                }
            }

            Log.Trace("Daemon::_Scheduler: leave");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Загружаем конфигурацию.
        /// </summary>
        public static void LoadConfiguration
            (
                string[] args
            )
        {
            Log.Trace("Daemon::LoadConfiguration: enter");

            WorkingInterval = ConfigurationUtility.GetInt32
                (
                    "interval",
                    5 * 60 * 1000
                );

            Log.Trace("Daemon::LoadConfiguration: exit");
        }

        /// <summary>
        /// Запускаем шедулер.
        /// </summary>
        public static void Start()
        {
            Log.Trace("Daemon::Start: enter");

            if (!Running)
            {
                Running = true;
                WorkThread = new Thread(_Scheduler);
            }

            Log.Trace("Daemon::Start: exit");
        }

        /// <summary>
        /// Дожидаемся оставновки.
        /// </summary>
        /// <param name="timeout">Сколько миллисекунд ждать.</param>
        public static void Stop
            (
                int timeout = -1
            )
        {
            Log.Trace("Daemon::Stop: enter");

            if (Running)
            {
                Running = false;
                if (timeout > 0)
                {
                    WorkThread.Join(timeout);
                }
                else
                {
                    WorkThread.Join();
                }
            }

            Log.Trace("Daemon:Stop: exit");
        }

        #endregion
    }
}
