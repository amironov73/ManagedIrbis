/* IrbisBusyManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Управляет отображением окна "ИРБИС занят, подождите".
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    // ReSharper disable once LocalizableElement
    [System.ComponentModel.DesignerCategory("Code")]
    public sealed class IrbisBusyManager
        : Component
    {
        #region Constants

        /// <summary>
        /// Default delay, milliseconds.
        /// </summary>
        public const int DelayConstant = 100;

        #endregion

        #region Properties

        /// <summary>
        /// Значение задержки по умолчанию.
        /// </summary>
        public static int DefaultDelay = DelayConstant;

        /// <summary>
        /// Занят ли сейчас клиент обращением к ИРБИС-серверу.
        /// </summary>
        public bool Busy { get { return Connection.Busy; } }

        /// <summary>
        /// Ссылка на клиент.
        /// </summary>
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Задержка между началом запроса к серверу
        /// и появлением окна "Ждите", миллисекунды.
        /// Нулевое или отрицательное значение - нет задержки.
        /// </summary>
        [DefaultValue(DelayConstant)]
        public int Delay { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisBusyManager
            (
                [NotNull] IrbisConnection connection
            )
        {
            Debug.WriteLine("ENTER IrbisBusyManager..ctor");

            if (ReferenceEquals(connection, null))
            {
                throw new ArgumentNullException("connection");
            }

            Delay = DefaultDelay;

            Connection = connection;
            Connection.Busy.StateChanged += _BusyChanged;
            Connection.Disposing += _ClientDisposing;
            
            Debug.WriteLine("LEAVE IrbisBusyManager..ctor");
        }

        #endregion

        #region Private members

        private Thread _uiThread;
        private IrbisBusyForm _waitForm;
        private bool _workDone;
        private ManualResetEvent _waitEvent;
        private static int _counter;
        private static string _formTitle;
        private static string _formMessage;

        private void _DebugThreadName()
        {
            Debug.WriteLine("THREAD=" + Thread.CurrentThread.Name);
        }

        private void _ClientDisposing
            (
                object sender,
                EventArgs e
            )
        {
            Debug.WriteLine("ENTER IrbisBusyManager._ClientDisposing");
            _DebugThreadName();

            Dispose(true);

            Debug.WriteLine("LEAVE IrbisBusyManager._ClientDisposing");
        }

        private void _BreakPressed
            (
                object sender,
                EventArgs e
            )
        {
            Debug.WriteLine("ENTER IrbisBusyManager._BreakPressed");
            _DebugThreadName();

            if (!ReferenceEquals(Connection, null))
            {
                // TODO Connection.Interrupted
                // Connection.Interrupted = true;
            }

            Debug.WriteLine("LEAVE IrbisBusyManager._BreakPressed");
        }

        private void _BusyChanged
            (
                object sender, 
                EventArgs e
            )
        {
            Debug.WriteLine("ENTER IrbisBusyManager._BusyChanged");
            Debug.WriteLine("BUSY=" + Busy);
            _DebugThreadName();

            if (!ReferenceEquals(sender, Connection))
            {
                throw new ApplicationException("Bad sender");
            }

            if (Busy)
            {
                // Началось обращение к серверу
                _counter++;
                _workDone = false;
                _uiThread = new Thread(_ThreadMethod1)
                {
                    IsBackground = true,
                    Name = "IrbisBusyManager" + _counter
                };
                _uiThread.SetApartmentState(ApartmentState.STA);
                _uiThread.Start();
            }
            else
            {
                // Обращение к серверу закончилось
                _workDone = true;
                if (!ReferenceEquals(_waitEvent, null))
                {
                    _waitEvent.Set();
                }
                _FormCleanup();
            }

            Debug.WriteLine("LEAVE IrbisBusyManager._BusyChanged");
        }

        /// <summary>
        /// Выполняется в отдельном потоке.
        /// Обратите внимание: созданная форма имеет
        /// отдельную очередь сообщений, поэтому не блокируется.
        /// Основной пользовательский интерфейс при этом
        /// может блокироваться клиентом.
        /// </summary>
        private void _ThreadMethod1()
        {
            Debug.WriteLine("ENTER IrbisBusyManager._ThreadMethod1");
            _DebugThreadName();

            if (Delay > 0)
            {
                _waitEvent = new ManualResetEvent(false);
                _waitEvent.WaitOne(Delay);
            }

            if (!_workDone)
            {
                try
                {
                    _waitForm = new IrbisBusyForm();
                    if (!ReferenceEquals(_formTitle, null))
                    {
                        _waitForm.SetTitle(_formTitle);
                    }
                    if (!ReferenceEquals(_formMessage, null))
                    {
                        _waitForm.SetMessage(_formMessage);
                    }
                    _waitForm.BreakPressed += _BreakPressed;
                    _waitForm.ShowDialog();
                }
                finally
                {
                    _FormCleanup();
                }
            }

            Debug.WriteLine("LEAVE IrbisBusyManager._ThreadMethod1");
        }

        private void _FormCleanup()
        {
            Debug.WriteLine("ENTER IrbisBusyManager._FormCleanup");
            _DebugThreadName();

            if (!ReferenceEquals(_waitForm, null))
            {
                if (_waitForm.InvokeRequired)
                {
                    _waitForm.Invoke((MethodInvoker) _FormCleanup);
                }
                else
                {
                    _waitForm.BreakPressed -= _BreakPressed;
                    _waitForm.Close();
                    //_waitForm = null; // не сметь обнулять!!!
                }
            }

            Debug.WriteLine("LEAVE IrbisBusyManager._FormCleanup");
        }

        private void _ClientCleanup()
        {
            Debug.WriteLine("ENTER IrbisBusyManager._ClientCleanup");
            _DebugThreadName();

            if (!ReferenceEquals(Connection, null))
            {
                Connection.Busy.StateChanged -= _BusyChanged;
                Connection.Disposing -= _ClientDisposing;
                Connection = null;
            }

            Debug.WriteLine("LEAVE IrbisBusyManager._ClientCleanup");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Установка заголовка окна.
        /// </summary>
        public void SetTitle
            (
                string title
            )
        {
            Debug.WriteLine("ENTER IrbisBusyManager.SetTitle");
            _DebugThreadName();

            _formTitle = title;

            if (!ReferenceEquals(_waitForm, null))
            {
                _waitForm.Invoke((MethodInvoker) (() => _waitForm.SetTitle(title)));
            }

            Debug.WriteLine("LEAVE IrbisBusyManager.SetTitle");
        }

        /// <summary>
        /// Установка надписи на форме.
        /// </summary>
        public void SetMessage
            (
                [NotNull] string message
            )
        {
            Code.NotNull(message, "message");

            Debug.WriteLine("ENTER IrbisBusyManager.SetMessage");
            _DebugThreadName();

            _formMessage = message;

            if (!ReferenceEquals(_waitForm, null))
            {
                _waitForm.Invoke((MethodInvoker) (() => _waitForm.SetMessage(message)));
            }

            Debug.WriteLine("LEAVE IrbisBusyManager.SetMessage");
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc />
        protected override void Dispose
            (
                bool disposing
            )
        {
            Debug.WriteLine("ENTER IrbisBusyManager.Dispose(bool)");
            _DebugThreadName();

            _FormCleanup();
            _ClientCleanup();

            base.Dispose(disposing);

            Debug.WriteLine("LEAVE IrbisBusyManager.Dispose(bool)");
        }

        #endregion
    }
}
