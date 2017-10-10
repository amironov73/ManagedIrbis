// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OfficialIrbisProvider.cs -- 
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
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace OfficialWrapper
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class OfficialIrbisProvider
        : IrbisProvider
    {
        #region Constants

        /// <summary>
        /// Provider name.
        /// </summary>
        public const string ProviderName = "Official";

        #endregion

        #region Properties

        /// <summary>
        /// Client.
        /// </summary>
        [NotNull]
        public Irbis64Client Client { get; private set; }

        /// <inheritdoc cref="IrbisProvider.Database" />
        public override string Database
        {
            get { return Client.Database; }
            set { Client.Database = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public OfficialIrbisProvider()
        {
            Client = new Irbis64Client();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Register the provider.
        /// </summary>
        public static void Register()
        {
            ProviderManager.Registry.Add
                (
                    ProviderName,
                    typeof(OfficialIrbisProvider)
                );
        }


        #endregion

        #region IrbisProvider members

        /// <inheritdoc cref="IrbisProvider.Configure" />
        public override void Configure
            (
                string configurationString
            )
        {
            Code.NotNull(configurationString, "configurationString");

            Client.ParseConnectionString(configurationString);
        }

        /// <inheritdoc cref="IrbisProvider.GetMaxMfn" />
        public override int GetMaxMfn()
        {
            return Client.GetMaxMfn();
        }

        /// <inheritdoc cref="IrbisProvider.ReadRecord" />
        public override MarcRecord ReadRecord
            (
                int mfn
            )
        {
            return base.ReadRecord(mfn);
        }

        /// <inheritdoc cref="IrbisProvider.Search" />
        public override int[] Search
            (
                string expression
            )
        {
            Code.NotNullNorEmpty(expression, "expression");

            return Client.Search(expression);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IrbisProvider.Dispose" />
        public override void Dispose()
        {
            Client.Dispose();
            base.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
