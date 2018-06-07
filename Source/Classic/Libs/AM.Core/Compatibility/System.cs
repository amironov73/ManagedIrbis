// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* System.cs -- temporary solution for .NET Core compatibility
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if WINMOBILE || PocketPC

using System;

namespace System
{
    /// <summary>
    /// Generic data converter.
    /// </summary>
    public delegate TOutput Converter<TInput, TOutput> (TInput input);

    ///// <summary>
    /////
    ///// </summary>
    //public sealed class NonSerializedAttribute
    //    : Attribute
    //{
    //}

    //public sealed class ArgumentOutOfRangeException
    //    : Exception
    //{
    //    public ArgumentOutOfRangeException()
    //    {
    //    }

    //    public ArgumentOutOfRangeException(string message) : base(message)
    //    {
    //    }

    //    public ArgumentOutOfRangeException(string message, Exception innerException) : base(message, innerException)
    //    {
    //    }

    //    public ArgumentOutOfRangeException(string paramName, object actualValue, string message)
    //        : base(message)
    //    {

    //    }
    //}
}

#endif

#if UAP || WINMOBILE || PocketPC

namespace System
{
    /// <summary>
    /// Console color.
    /// </summary>
    public enum ConsoleColor
    {
        /// <summary>
        ///
        /// </summary>
        Black,

        /// <summary>
        ///
        /// </summary>
        Blue,

        /// <summary>
        ///
        /// </summary>
        Cyan,

        /// <summary>
        ///
        /// </summary>
        DarkBlue,

        /// <summary>
        ///
        /// </summary>
        DarkCyan,

        /// <summary>
        ///
        /// </summary>
        DarkGray,

        /// <summary>
        ///
        /// </summary>
        DarkGreen,

        /// <summary>
        ///
        /// </summary>
        DarkMagenta,

        /// <summary>
        ///
        /// </summary>
        DarkRed,

        /// <summary>
        ///
        /// </summary>
        DarkYellow,

        /// <summary>
        ///
        /// </summary>
        Gray,

        /// <summary>
        ///
        /// </summary>
        Green,

        /// <summary>
        ///
        /// </summary>
        Magenta,

        /// <summary>
        ///
        /// </summary>
        Red,

        /// <summary>
        ///
        /// </summary>
        White,

        /// <summary>
        ///
        /// </summary>
        Yellow
    }

    /// <summary>
    /// Console key
    /// </summary>
    public enum ConsoleKey
    {
        /// <summary>
        ///
        /// </summary>
        A,

        /// <summary>
        ///
        /// </summary>
        Add,

        /// <summary>
        ///
        /// </summary>
        Applications,

        /// <summary>
        ///
        /// </summary>
        Attention,

        /// <summary>
        ///
        /// </summary>
        B,

        /// <summary>
        ///
        /// </summary>
        Backspace,

        /// <summary>
        ///
        /// </summary>
        BrowserBack,

        /// <summary>
        ///
        /// </summary>
        BrowserFavorites,

        /// <summary>
        ///
        /// </summary>
        BrowserForward,

        /// <summary>
        ///
        /// </summary>
        BrowserHome,

        /// <summary>
        ///
        /// </summary>
        BrowserRefresh,

        /// <summary>
        ///
        /// </summary>
        BrowserSearch,

        /// <summary>
        ///
        /// </summary>
        BrowserStop,

        /// <summary>
        ///
        /// </summary>
        C,

        /// <summary>
        ///
        /// </summary>
        Clear,

        /// <summary>
        ///
        /// </summary>
        CrSel,

        /// <summary>
        ///
        /// </summary>
        D,

        /// <summary>
        ///
        /// </summary>
        D0,

        /// <summary>
        ///
        /// </summary>
        D1,

        /// <summary>
        ///
        /// </summary>
        D2,

        /// <summary>
        ///
        /// </summary>
        D3,

        /// <summary>
        ///
        /// </summary>
        D4,

        /// <summary>
        ///
        /// </summary>
        D5,

        /// <summary>
        ///
        /// </summary>
        D6,

        /// <summary>
        ///
        /// </summary>
        D7,

        /// <summary>
        ///
        /// </summary>
        D8,

        /// <summary>
        ///
        /// </summary>
        D9,

        /// <summary>
        ///
        /// </summary>
        Decimal,

        /// <summary>
        ///
        /// </summary>
        Delete,

        /// <summary>
        ///
        /// </summary>
        Divide,

        /// <summary>
        ///
        /// </summary>
        DownArrow,

        /// <summary>
        ///
        /// </summary>
        E,

        /// <summary>
        ///
        /// </summary>
        End,

        /// <summary>
        ///
        /// </summary>
        Enter,

        /// <summary>
        ///
        /// </summary>
        EraseEndOfFile,

        /// <summary>
        ///
        /// </summary>
        Escape,

        /// <summary>
        ///
        /// </summary>
        Execute,

        /// <summary>
        ///
        /// </summary>
        ExSel,

        /// <summary>
        ///
        /// </summary>
        F,

        /// <summary>
        ///
        /// </summary>
        F1,

        /// <summary>
        ///
        /// </summary>
        F10,

        /// <summary>
        ///
        /// </summary>
        F11,

        /// <summary>
        ///
        /// </summary>
        F12,

        /// <summary>
        ///
        /// </summary>
        F13,

        /// <summary>
        ///
        /// </summary>
        F14,

        /// <summary>
        ///
        /// </summary>
        F15,

        /// <summary>
        ///
        /// </summary>
        F16,

        /// <summary>
        ///
        /// </summary>
        F17,

        /// <summary>
        ///
        /// </summary>
        F18,

        /// <summary>
        ///
        /// </summary>
        F19,

        /// <summary>
        ///
        /// </summary>
        F2,

        /// <summary>
        ///
        /// </summary>
        F20,

        /// <summary>
        ///
        /// </summary>
        F21,

        /// <summary>
        ///
        /// </summary>
        F22,

        /// <summary>
        ///
        /// </summary>
        F23,

        /// <summary>
        ///
        /// </summary>
        F24,

        /// <summary>
        ///
        /// </summary>
        F3,

        /// <summary>
        ///
        /// </summary>
        F4,

        /// <summary>
        ///
        /// </summary>
        F5,

        /// <summary>
        ///
        /// </summary>
        F6,

        /// <summary>
        ///
        /// </summary>
        F7,

        /// <summary>
        ///
        /// </summary>
        F8,

        /// <summary>
        ///
        /// </summary>
        F9,

        /// <summary>
        ///
        /// </summary>
        G,

        /// <summary>
        ///
        /// </summary>
        H,

        /// <summary>
        ///
        /// </summary>
        Help,

        /// <summary>
        ///
        /// </summary>
        Home,

        /// <summary>
        ///
        /// </summary>
        I,

        /// <summary>
        ///
        /// </summary>
        Insert,

        /// <summary>
        ///
        /// </summary>
        J,

        /// <summary>
        ///
        /// </summary>
        K,

        /// <summary>
        ///
        /// </summary>
        L,

        /// <summary>
        ///
        /// </summary>
        LaunchApp1,

        /// <summary>
        ///
        /// </summary>
        LaunchApp2,

        /// <summary>
        ///
        /// </summary>
        LaunchMail,

        /// <summary>
        ///
        /// </summary>
        LaunchMediaSelect,

        /// <summary>
        ///
        /// </summary>
        LeftArrow,

        /// <summary>
        ///
        /// </summary>
        LeftWindows,

        /// <summary>
        ///
        /// </summary>
        M,

        /// <summary>
        ///
        /// </summary>
        MediaNext,

        /// <summary>
        ///
        /// </summary>
        MediaPlay,

        /// <summary>
        ///
        /// </summary>
        MediaPrevious,

        /// <summary>
        ///
        /// </summary>
        MediaStop,

        /// <summary>
        ///
        /// </summary>
        Multiply,

        /// <summary>
        ///
        /// </summary>
        N,

        /// <summary>
        ///
        /// </summary>
        NoName,

        /// <summary>
        ///
        /// </summary>
        NumPad0,

        /// <summary>
        ///
        /// </summary>
        NumPad1,

        /// <summary>
        ///
        /// </summary>
        NumPad2,

        /// <summary>
        ///
        /// </summary>
        NumPad3,

        /// <summary>
        ///
        /// </summary>
        NumPad4,

        /// <summary>
        ///
        /// </summary>
        NumPad5,

        /// <summary>
        ///
        /// </summary>
        NumPad6,

        /// <summary>
        ///
        /// </summary>
        NumPad7,

        /// <summary>
        ///
        /// </summary>
        NumPad8,

        /// <summary>
        ///
        /// </summary>
        NumPad9,

        /// <summary>
        ///
        /// </summary>
        O,

        /// <summary>
        ///
        /// </summary>
        Oem1,

        /// <summary>
        ///
        /// </summary>
        Oem102,

        /// <summary>
        ///
        /// </summary>
        Oem2,

        /// <summary>
        ///
        /// </summary>
        Oem3,

        /// <summary>
        ///
        /// </summary>
        Oem4,

        /// <summary>
        ///
        /// </summary>
        Oem5,

        /// <summary>
        ///
        /// </summary>
        Oem6,

        /// <summary>
        ///
        /// </summary>
        Oem7,

        /// <summary>
        ///
        /// </summary>
        Oem8,

        /// <summary>
        ///
        /// </summary>
        OemClear,

        /// <summary>
        ///
        /// </summary>
        OemComma,

        /// <summary>
        ///
        /// </summary>
        OemMinus,

        /// <summary>
        ///
        /// </summary>
        OemPeriod,

        /// <summary>
        ///
        /// </summary>
        OemPlus,

        /// <summary>
        ///
        /// </summary>
        P,

        /// <summary>
        ///
        /// </summary>
        Pa1,

        /// <summary>
        ///
        /// </summary>
        Packet,

        /// <summary>
        ///
        /// </summary>
        PageDown,

        /// <summary>
        ///
        /// </summary>
        PageUp,

        /// <summary>
        ///
        /// </summary>
        Pause,

        /// <summary>
        ///
        /// </summary>
        Play,

        /// <summary>
        ///
        /// </summary>
        Print,

        /// <summary>
        ///
        /// </summary>
        PrintScreen,

        /// <summary>
        ///
        /// </summary>
        Process,

        /// <summary>
        ///
        /// </summary>
        Q,

        /// <summary>
        ///
        /// </summary>
        R,

        /// <summary>
        ///
        /// </summary>
        RightArrow,

        /// <summary>
        ///
        /// </summary>
        RightWindows,

        /// <summary>
        ///
        /// </summary>
        S,

        /// <summary>
        ///
        /// </summary>
        Select,

        /// <summary>
        ///
        /// </summary>
        Separator,

        /// <summary>
        ///
        /// </summary>
        Sleep,

        /// <summary>
        ///
        /// </summary>
        Spacebar,

        /// <summary>
        ///
        /// </summary>
        Subtract,

        /// <summary>
        ///
        /// </summary>
        T,

        /// <summary>
        ///
        /// </summary>
        Tab,

        /// <summary>
        ///
        /// </summary>
        U,

        /// <summary>
        ///
        /// </summary>
        UpArrow,

        /// <summary>
        ///
        /// </summary>
        V,

        /// <summary>
        ///
        /// </summary>
        VolumeDown,

        /// <summary>
        ///
        /// </summary>
        VolumeMute,

        /// <summary>
        ///
        /// </summary>
        VolumeUp,

        /// <summary>
        ///
        /// </summary>
        W,

        /// <summary>
        ///
        /// </summary>
        X,

        /// <summary>
        ///
        /// </summary>
        Y,

        /// <summary>
        ///
        /// </summary>
        Z,

        /// <summary>
        ///
        /// </summary>
        Zoom
    }

    /// <summary>
    ///
    /// </summary>
    [FlagsAttribute]
    public enum ConsoleModifiers
    {
        /// <summary>
        ///
        /// </summary>
        Alt = 0x01,

        /// <summary>
        ///
        /// </summary>
        Control = 0x02,

        /// <summary>
        ///
        /// </summary>
        Shift = 0x04
    }

    /// <summary>
    ///
    /// </summary>
    public struct ConsoleKeyInfo
    {
        /// <summary>
        /// Key.
        /// </summary>
        public ConsoleKey Key { get; set; }

        /// <summary>
        /// Char.
        /// </summary>
        public char KeyChar { get; set; }

        /// <summary>
        /// Modifiers.
        /// </summary>
        public ConsoleModifiers Modifiers { get; set; }
    }
}

#endif

#if UAP

namespace System
{
    /// <summary>
    /// Supports cloning, which creates a new instance
    /// of a class with the same value as an existing instance.
    /// </summary>
    public interface ICloneable
    {
        /// <summary>
        /// Creates a new object that is a copy
        /// of the current instance.
        /// </summary>
        object Clone ();
    }

    /// <summary>
    /// Represents a method that converts an object from one type to another type.
    /// </summary>
    public delegate TOutput Converter<TInput, TOutput>
        (
            TInput input
        );

    /// <summary>
    /// For compatibility.
    /// </summary>
    public enum ConsoleSpecialKey
    {
        // We realize this is incomplete, and may add values in the future.

        /// <summary>
        /// For compatibility.
        /// </summary>
        ControlC = 0,

        /// <summary>
        /// For compatibility.
        /// </summary>
        ControlBreak = 1,
    }

    /// <summary>
    /// For compatibility.
    /// </summary>
    public sealed class ConsoleCancelEventArgs : EventArgs
    {
        private ConsoleSpecialKey _type;
        private bool _cancel;  // Whether to cancel the CancelKeyPress event

        internal ConsoleCancelEventArgs(ConsoleSpecialKey type)
        {
            _type = type;
            _cancel = false;
        }

        // Whether to cancel the break event.  By setting this to true, the
        // Control-C will not kill the process.

        /// <summary>
        /// For compatibility.
        /// </summary>
        public bool Cancel
        {
            get { return _cancel; }
            set {
                _cancel = value;
            }
        }

        /// <summary>
        /// For compatibility.
        /// </summary>
        public ConsoleSpecialKey SpecialKey
        {
            get { return _type; }
        }
    }
}

#endif
