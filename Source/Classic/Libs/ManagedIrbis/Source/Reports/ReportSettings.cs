// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportSettings.cs -- 
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

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReportSettings
    {
        #region Properties

        /// <summary>
        /// Assemblies to load.
        /// </summary>
        [NotNull]
        [JsonProperty("assemblies")]
        public NonNullCollection<string> Assemblies { get; private set; }

        /// <summary>
        /// Name of the <see cref="ReportDriver"/>.
        /// </summary>
        [CanBeNull]
        [JsonProperty("driver")]
        public string DriverName { get; set; }

        /// <summary>
        /// Settings for driver.
        /// </summary>
        [CanBeNull]
        [JsonProperty("driverSettings")]
        public string DriverSettings { get; set; }

        /// <summary>
        /// Output file name.
        /// </summary>
        [CanBeNull]
        [JsonProperty("outputFile")]
        public string OutputFile { get; set; }

        /// <summary>
        /// Page settings.
        /// </summary>
        [CanBeNull]
        [JsonProperty]
        public string PageSettings { get; set; }

        /// <summary>
        /// Printer to send report to.
        /// </summary>
        [CanBeNull]
        [JsonProperty("printer")]
        public string PrinterName { get; set; }

        /// <summary>
        /// Name of <see cref="IrbisProvider"/>.
        /// </summary>
        [CanBeNull]
        [JsonProperty("providerName")]
        public string ProviderName { get; set; }

        /// <summary>
        /// Settings for provider.
        /// </summary>
        [CanBeNull]
        [JsonProperty("providerSettings")]
        public string ProviderSettings { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportSettings()
        {
            Assemblies = new NonNullCollection<string>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
