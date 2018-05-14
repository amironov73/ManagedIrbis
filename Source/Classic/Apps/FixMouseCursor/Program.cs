using System;
using System.Runtime.InteropServices;

namespace FixMouseCursor
{
    static class Program
    {
        [DllImport("user32.dll")]
        public static extern bool SystemParametersInfo
            (
                int uiAction,
                int uiParam,
                IntPtr pvParam,
                int fWinIni
            );

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SystemParametersInfo(/*SPI_SETCURSORS*/ 0x0057, 0, IntPtr.Zero, 0);
        }
    }
}
