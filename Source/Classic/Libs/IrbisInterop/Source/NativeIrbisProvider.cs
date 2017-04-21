// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NativeIrbisProvider.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Server;

using MoonSharp.Interpreter;

#endregion

namespace IrbisInterop.Source
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class NativeIrbisProvider
        : IrbisProvider
    {
        #region Properties

        /// <summary>
        /// IRBIS64.DLL wrapper.
        /// </summary>
        [NotNull]
        public Irbis64Dll Irbis64 { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NativeIrbisProvider
            (
                [NotNull] Irbis64Dll irbis64
            )
        {
            Code.NotNull(irbis64, "irbis64");

            Irbis64 = irbis64;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NativeIrbisProvider
            (
                [NotNull] ServerConfiguration configuration
            )
        {
            Code.NotNull(configuration, "configuration");

            Irbis64 = new Irbis64Dll(configuration);
            _ownIrbis64 = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NativeIrbisProvider
            (
                [NotNull] string serverIniPath
            )
            : this (ServerConfiguration.FromIniFile(serverIniPath))
        {
        }

        #endregion

        #region Private members

        private bool _ownIrbis64;

        #endregion

        #region Public methods

        #endregion

        #region IrbisProvider members

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            if (_ownIrbis64)
            {
                Irbis64.Dispose();
            }
            base.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
