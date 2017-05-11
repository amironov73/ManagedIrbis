// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Isis32Dll.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Isis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Isis32Dll
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// ISIS application handle.
        /// </summary>
        public int AppHandle { get; private set; }

        /// <summary>
        /// FST file name.
        /// </summary>
        [CanBeNull]
        public string FstFileName { get; private set; }

        /// <summary>
        /// Name of the inverted file.
        /// </summary>
        [CanBeNull]
        public string InvertedFileName { get; private set; }

        /// <summary>
        /// Name of the master file.
        /// </summary>
        [CanBeNull]
        public string MasterFileName { get; private set; }

        /// <summary>
        /// PFT file name.
        /// </summary>
        [CanBeNull]
        public string PftFileName { get; private set; }

        /// <summary>
        /// Shelf number.
        /// </summary>
        public int ShelfNumber { get; set; }

        /// <summary>
        /// ISIS space handle.
        /// </summary>
        public int SpaceHandle { get; private set; }

        /// <summary>
        /// Get version of the ISIS32.DLL.
        /// </summary>
        [NotNull]
        public static string Version
        {
            get
            {
                float ver = IsisInterop.IsisDllVersion();
                return ver.ToString
                    (
                        "##.##",
                        CultureInfo.InvariantCulture
                    );
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Isis32Dll()
        {
            Log.Trace("Isis32Dll::Constructor");

            AppHandle = IsisInterop.IsisAppNew();
            CheckResult(AppHandle);
            SpaceHandle = IsisInterop.IsisSpaNew(AppHandle);
            CheckResult(SpaceHandle);

            IsisInterop.IsisAppDebug
                (
                    AppHandle,
                    IsisDebugFlags.DEBUG_VERY_LIGHT
                );
        }

        #endregion

        #region Private members

        private bool _disposed;

        private void CheckDisposed()
        {
            if (_disposed)
            {
                Log.Trace
                    (
                        "Isis32Dll::CheckDisposed: disposed="
                        + _disposed
                    );
                throw new ObjectDisposedException("Isis32Dll");
            }
        }

        private static void CheckResult
            (
                int isisCode
            )
        {
            if (isisCode < 0)
            {
                Log.Trace
                    (
                        "Isis32Dll::CheckResult: isisCode=" 
                        + isisCode
                    );
                throw new IsisException(isisCode);
            }
        }

        private static void CheckResult
            (
                int isisCode,
                int mfn
            )
        {
            if (isisCode < 0)
            {
                Log.Trace
                    (
                        "Isis32Dll::CheckResult: isisCode="
                        + isisCode
                        + ", mfn="
                        + mfn
                    );
                throw new IsisException(isisCode, mfn);
            }
        }

        #endregion

        #region Public methods



        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Log.Trace("Isis32Dll::Dispose");

            if (SpaceHandle != 0)
            {
                IsisInterop.IsisSpaDelete(SpaceHandle);
            }
            if (AppHandle != 0)
            {
                IsisInterop.IsisAppDelete(AppHandle);
            }
            _disposed = true;
        }

        #endregion
    }
}
