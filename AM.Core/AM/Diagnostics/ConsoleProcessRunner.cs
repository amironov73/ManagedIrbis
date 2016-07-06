/* ConsoleProcessRunner.cs
 * ArsMagna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;

using CodeJam;

using JetBrains.Annotations;

using AM.IO;

using MoonSharp.Interpreter;

#endregion

namespace AM.Diagnostics
{
    /// <summary>
    /// Runs console process and intercepts its output
    /// redirecting to text box.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConsoleProcessRunner
    {
        #region Properties

        /// <summary>
        /// Gets the receiver.
        /// </summary>
        public IConsoleOutputReceiver Receiver
        {
            get
            {
                return _receiver;
            }
        }

        /// <summary>
        /// Gets the running process.
        /// </summary>
        public Process RunningProcess
        {
            get
            {
                return _runningProcess;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ConsoleProcessRunner"/> class.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        public ConsoleProcessRunner
            (
                [NotNull] IConsoleOutputReceiver receiver
            )
        {
            Code.NotNull(receiver, "receiver");

            _receiver = receiver;
        }

        #endregion

        #region Private members

        private readonly IConsoleOutputReceiver _receiver;

        private Process _runningProcess;

        private void _OutputDataReceived
            (
                object sender,
                DataReceivedEventArgs e
            )
        {
            if ((Receiver != null)
                 && (e != null)
                 && (e.Data != null))
            {
                Receiver.ReceiveConsoleOutput(e.Data);
            }
        }

        private void _ProcessExited
            (
            object sender,
            EventArgs e)
        {
            if (RunningProcess != null)
            {
                RunningProcess.OutputDataReceived -= _OutputDataReceived;
                RunningProcess.Exited -= _ProcessExited;
                _runningProcess = null;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts new process with the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        public void Start
            (
            string fileName,
            string arguments)
        {
            if ((RunningProcess != null)
                 && !RunningProcess.HasExited)
            {
                throw new ArsMagnaException();
            }
            ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    
                    // ReSharper disable AssignNullToNotNullAttribute
                    StandardErrorEncoding = null,
                    
                    // If the value of the StandardOutputEncoding property 
                    // is null, the process uses the default standard 
                    // output encoding for the standard output.
                    StandardOutputEncoding = null,
                    
                    // ReSharper restore AssignNullToNotNullAttribute
                    
                    UseShellExecute = false
                };
            _runningProcess = new Process
                {
                    StartInfo = startInfo
                    //, SynchronizingObject = Receiver // Use this to event handler calls 
                    // that are issued as a result of an Exited event on the process
                };
           ISynchronizeInvoke synchronizingObject =
                    Receiver as ISynchronizeInvoke;
           if (synchronizingObject != null)
           {
                _runningProcess.SynchronizingObject = synchronizingObject;
           }
            _runningProcess.OutputDataReceived += _OutputDataReceived;
            _runningProcess.ErrorDataReceived += _OutputDataReceived;
            _runningProcess.Exited += _ProcessExited;
            _runningProcess.Start();
            _runningProcess.BeginOutputReadLine();
            _runningProcess.BeginErrorReadLine();
        }

        /// <summary>
        /// Stops running process if any.
        /// </summary>
        public void Stop()
        {
            if ((RunningProcess != null)
                 && !RunningProcess.HasExited)
            {
                RunningProcess.OutputDataReceived -= _OutputDataReceived;
                RunningProcess.Exited -= _ProcessExited;
            }
        }

        #endregion
    }
}
