/* DynamicLibrary.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion


namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// Dynamic library (DLL) of Windows.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DynamicLibrary
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Library name.
        /// </summary>
        [NotNull]
        public string LibraryName { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DynamicLibrary
            (
                [NotNull] string libraryName
            )
        {
            Code.NotNullNorEmpty(libraryName, "libraryName");

            LibraryName = libraryName;

            _handle = LoadLibrary(libraryName);
            if (_handle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new ArsMagnaException
                    (
                        string.Format
                        (
                            "Failed to load library (ErrorCode: {0})",
                            errorCode
                        )
                    );
            }
        }

        #endregion

        #region Private members

        private readonly IntPtr _handle;

        #region Native methods

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi,
            SetLastError = true)]
        private static extern IntPtr LoadLibrary
            (
                string libraryName
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern bool FreeLibrary
            (
                IntPtr hModule
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress
            (
                IntPtr hModule,
                string lpProcName
            );

        #endregion


        #endregion

        #region Public methods

        /// <summary>
        /// Create delegate for specified function.
        /// </summary>
        [NotNull]
        public Delegate CreateDelegate
            (
                [NotNull] string functionName,
                [NotNull] Type type
            )
        {
            Code.NotNullNorEmpty(functionName, "functionName");
            Code.NotNull(type, "type");

            IntPtr address = GetProcAddress
                (
                    _handle,
                    functionName
                );
            if (address == IntPtr.Zero)
            {
                throw new ArsMagnaException("Can't find function");
            }

            Delegate result = Marshal.GetDelegateForFunctionPointer
                (
                    address,
                    type
                );

            return result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc/>
        public void Dispose()
        {
            FreeLibrary(_handle);
        }

        #endregion
    }
}

#endif
