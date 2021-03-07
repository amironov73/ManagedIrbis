// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Reflection;

using NLog;
using NLog.Config;

#endregion

namespace AM.Logging.NLog
{
    //
    // NLog log levels
    //
    // * Fatal - something bad happened; application is going down
    // * Error - something failed; application may or may not continue
    // * Warn  - something unexpected; application will continue
    // * Info  - normal behavior like mail sent, user updated profile etc.
    // * Debug - for debugging; executed query, user authenticated, session expired
    // * Trace - for trace debugging; begin method X, end method X
    //

    /// <summary>
    /// Logger for NLog.
    /// </summary>
    public sealed class NLogger
        : IAmLogger
    {
        #region Properties

        /// <summary>
        /// Inner logger.
        /// </summary>
        public static Logger Logger { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// 
        /// </summary>
        public NLogger()
        {
            if (ReferenceEquals(Logger, null))
            {
                lock (SyncRoot)
                {
                    if (ReferenceEquals(Logger, null))
                    {
                        string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                            .ThrowIfNull("assemblyFolder");
                        string configFile = Path.Combine(assemblyFolder, "NLog.config");
                        LogManager.Configuration = new XmlLoggingConfiguration(configFile, false);

                        Logger = LogManager.GetCurrentClassLogger();
                    }
                }
            }
        }

        #endregion

        #region Private members

        private static readonly object SyncRoot = new object();

        #endregion

        #region Public methods

        #endregion

        #region IAMLogger members

        /// <inheritdoc />
        public void Debug
            (
                string text
            )
        {
            Logger.Debug(text);
        }

        /// <inheritdoc />
        public void Error
            (
                string text
            )
        {
            Logger.Error(text);
        }

        /// <inheritdoc />
        public void Fatal
            (
                string text
            )
        {
            Logger.Fatal(text);
        }

        /// <inheritdoc />
        public void Info
            (
                string text
            )
        {
            Logger.Info(text);
        }

        /// <inheritdoc />
        public void Trace
            (
                string text
            )
        {
            Logger.Trace(text);
        }

        /// <inheritdoc />
        public void Warn
            (
                string text
            )
        {
            Logger.Warn(text);
        }

        #endregion
    }
}
