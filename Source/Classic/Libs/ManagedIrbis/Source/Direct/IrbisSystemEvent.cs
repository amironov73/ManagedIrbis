// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisSystemEvent.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading;

using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    //
    // При старте сервера
    // 
    // IrbisSystemEvent systemEvent = new IrbisSystemEvent();
    // if (!systemEvent.CheckOtherServerRunning())
    // {
    //   Облом
    // }
    // systemEvent.SayIamRunning();
    //
    // В конце
    //
    // systemEvent.Dispose();
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisSystemEvent
        : IDisposable
    {
        #region Constants

        /// <summary>
        /// Name of the event.
        /// </summary>
        public const string StartedName = "IRBIS64_STARTED";

        /// <summary>
        /// Name of the event.
        /// </summary>
        public const string StopName = "IRBIS64_STOP_";

        #endregion

        #region Properties

        /// <summary>
        /// IRBIS64 server started.
        /// </summary>
        [NotNull]
        public EventWaitHandle StartedEvent { get; private set; }

        /// <summary>
        /// Stop the IRBIS64 server.
        /// </summary>
        [NotNull]
        public EventWaitHandle StopEvent { get; private set; }

        /// <summary>
        /// New event created.
        /// </summary>
        public bool Created { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisSystemEvent()
        {
            bool created;

            StartedEvent = new EventWaitHandle
                (
                    false, 
                    EventResetMode.ManualReset,
                    StartedName,
                    out created
                );
            Created = created;

            Log.Trace
                (
                    "IrbisSystemEvent::Constructor: "
                    + StartedName
                    + ": created="
                    + created
                );

            StopEvent = new EventWaitHandle
                (
                    false,
                    EventResetMode.ManualReset,
                    StopName
                );
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~IrbisSystemEvent()
        {
            Log.Trace
                (
                    "IrbisSystemEvent::Destructor"
                );

            Dispose();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Check if other IRBIS64 server is running.
        /// </summary>
        public bool CheckOtherServerRunning()
        {
            return !StartedEvent.WaitOne(1);
        }

        /// <summary>
        /// Whether the IRBIS64 needs to stop.
        /// </summary>
        public bool CheckStopRequested()
        {
            return StopEvent.WaitOne(1);
        }

        /// <summary>
        /// Request stop.
        /// </summary>
        public void RequestStop()
        {
            StopEvent.Set();
        }

        /// <summary>
        /// Say "I am running" to other servers (if any).
        /// </summary>
        public void SayIamRunning()
        {
            StartedEvent.Set();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Log.Trace
                (
                    "IrbisSystemEvent::Dispose"
                );

            GC.SuppressFinalize(this);

            StartedEvent.Dispose();
            StopEvent.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
