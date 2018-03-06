// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HotKey.cs -- 
   Ars Magna project, http://arsmagna.ru */

#if !NETCORE

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Hot key.
    /// </summary>
    public sealed class HotKey
        : Component,
        IMessageFilter
    {
        #region Properties

        private IntPtr _windowHandle;

        ///<summary>
        /// 
        ///</summary>
        [Browsable(false)]
        public IntPtr WindowHandle
        {
            [DebuggerStepThrough]
            get
            {
                return _windowHandle;
            }
            [DebuggerStepThrough]
            set
            {
                _windowHandle = value;
            }
        }

        private HotkeyModifiers _modifiers = HotkeyModifiers.None;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(HotkeyModifiers.None)]
        public HotkeyModifiers Modifiers
        {
            [DebuggerStepThrough]
            get
            {
                return _modifiers;
            }
            [DebuggerStepThrough]
            set
            {
                _modifiers = value;
            }
        }

        private Keys _key = Keys.A;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(Keys.A)]
        public Keys Key
        {
            [DebuggerStepThrough]
            get
            {
                return _key;
            }
            [DebuggerStepThrough]
            set
            {
                _key = value;
            }
        }

        private int _id;

        ///<summary>
        /// 
        ///</summary>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public int ID
        {
            [DebuggerStepThrough]
            get
            {
                return _id;
            }
        }

        private bool _active;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(false)]
        public bool Active
        {
            [DebuggerStepThrough]
            get
            {
                return _active;
            }
            [DebuggerStepThrough]
            set
            {
                _active = value;
                if (_active)
                {
                    _Initialize();
                }
                else
                {
                    _Deinitialize();
                }
            }
        }

        /// <summary>
        /// Fired when hot key pressed.
        /// </summary>
        public event EventHandler Pressed;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public HotKey()
        {
            _id = ++_counter;
            Application.AddMessageFilter(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent"></param>
        public HotKey(Container parent) : this()
        {
            if ((parent == null) || (parent is IWin32Window))
            {
                throw new ApplicationException();
            }
            _windowHandle = (parent as IWin32Window).Handle;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~HotKey()
        {
            Dispose();
        }

        #endregion

        #region Private members

        private static int _counter;

        private bool _initialized;

        private void _Initialize()
        {
            if (!DesignMode && !_initialized)
            {
                if (!User32.RegisterHotKey(_windowHandle, _id,
                    _modifiers, _key))
                {
                    throw new ApplicationException();
                }
                GC.ReRegisterForFinalize(this);
                _initialized = true;
            }
        }

        private void _Deinitialize()
        {
            if (!DesignMode && _initialized)
            {
                User32.UnregisterHotKey(_windowHandle, _id);
                GC.SuppressFinalize(this);
                _initialized = false;
            }
        }

        #endregion

        #region Component members

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            Application.RemoveMessageFilter(this);
            _Deinitialize();
            base.Dispose(disposing);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Fired when hot key pressed.
        /// </summary>
        public void OnPressed()
        {
            if (Pressed != null)
            {
                Pressed(this, EventArgs.Empty);
            }
        }

        #endregion

        #region IMessageFilter members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool PreFilterMessage(ref Message m)
        {
            if ((m.Msg == (int)WindowMessage.WM_HOTKEY)
                && (m.WParam.ToInt32() == _id))
            {
                OnPressed();
            }
            return false;
        }

        #endregion
    }
}

#endif
