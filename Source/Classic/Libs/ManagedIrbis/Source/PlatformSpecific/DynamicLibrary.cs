// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
    public class DynamicLibrary
        : IDisposable
    {
        #region Constants

        private const string Kernel32 = "kernel32.dll";

        #endregion

        #region Events

        /// <summary>
        /// Raised when disposing.
        /// </summary>
        public event EventHandler Disposing;

        #endregion

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

        /// <summary>
        /// Loads the specified module into the address space
        /// of the calling process. The specified module may
        /// cause other modules to be loaded.
        /// </summary>
        /// <param name="libraryName">The name of the module.
        /// This can be either a library module (a .dll file)
        /// or an executable module (an .exe file).</param>
        /// <returns>If the function succeeds, the return value
        /// is a handle to the module.
        /// If the function fails, the return value is NULL.
        /// To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms684175(v=vs.85).aspx
        /// </remarks>
        [DllImport(Kernel32, CharSet = CharSet.Ansi,
            SetLastError = true)]
        private static extern IntPtr LoadLibrary
            (
                string libraryName
            );

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and,
        /// if necessary, decrements its reference count.
        /// When the reference count reaches zero, the module
        /// is unloaded from the address space of the calling
        /// process and the handle is no longer valid.
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module.
        /// The LoadLibrary, LoadLibraryEx, GetModuleHandle,
        /// or GetModuleHandleEx function returns this handle.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. 
        /// To get extended error information, call the GetLastError function.
        /// </returns>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms683152(v=vs.85).aspx
        /// </remarks>
        [DllImport(Kernel32, CharSet = CharSet.Ansi)]
        private static extern bool FreeLibrary
            (
                IntPtr hModule
            );

        /// <summary>
        /// Retrieves the address of an exported function
        /// or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">A handle to the DLL module
        /// that contains the function or variable.
        /// The LoadLibrary, LoadLibraryEx, LoadPackagedLibrary,
        /// or GetModuleHandle function returns this handle.
        /// </param>
        /// <param name="lpProcName">The function or variable name,
        /// or the function's ordinal value. If this parameter
        /// is an ordinal value, it must be in the low-order word;
        /// the high-order word must be zero.</param>
        /// <returns>If the function succeeds, the return value
        /// is the address of the exported function or variable.
        /// If the function fails, the return value is NULL.
        /// To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms683212(v=vs.85).aspx
        /// </remarks>
        [DllImport(Kernel32, CharSet = CharSet.Ansi)]
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
            Disposing.Raise(this);
            FreeLibrary(_handle);
        }

        #endregion
    }
}

#endif
