// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportSettings.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#if !WINMOBILE

using AM.Json;

#endif

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReportSettings
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Assemblies to load.
        /// </summary>
        [NotNull]
        [JsonProperty("assemblies")]
        public NonNullCollection<string> Assemblies
        {
            get; private set;
        }

        /// <summary>
        /// Name of the <see cref="ReportDriver"/>.
        /// </summary>
        [CanBeNull]
        [JsonProperty("driver", NullValueHandling = NullValueHandling.Ignore)]
        public string DriverName { get; set; }

        /// <summary>
        /// Settings for driver.
        /// </summary>
        [CanBeNull]
        [JsonProperty("driverSettings", NullValueHandling = NullValueHandling.Ignore)]
        public string DriverSettings { get; set; }

        /// <summary>
        /// Record filter.
        /// </summary>
        [CanBeNull]
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public string Filter { get; set; }

        /// <summary>
        /// Output file name.
        /// </summary>
        [CanBeNull]
        [JsonProperty("outputFile", NullValueHandling = NullValueHandling.Ignore)]
        public string OutputFile { get; set; }

        /// <summary>
        /// Page settings.
        /// </summary>
        [CanBeNull]
        [JsonProperty("pageSettings", NullValueHandling = NullValueHandling.Ignore)]
        public string PageSettings { get; set; }

        /// <summary>
        /// Printer to send report to.
        /// </summary>
        [CanBeNull]
        [JsonProperty("printer", NullValueHandling = NullValueHandling.Ignore)]
        public string PrinterName { get; set; }

        /// <summary>
        /// Name of <see cref="IrbisProvider"/>.
        /// </summary>
        [CanBeNull]
        [JsonProperty("providerName", NullValueHandling = NullValueHandling.Ignore)]
        public string ProviderName { get; set; }

        /// <summary>
        /// Settings for provider.
        /// </summary>
        [CanBeNull]
        [JsonProperty("providerSettings", NullValueHandling = NullValueHandling.Ignore)]
        public string ProviderSettings { get; set; }

        /// <summary>
        /// Register <see cref="ReportDriver"/>
        /// before report building.
        /// </summary>
        [CanBeNull]
        [JsonProperty("registerDriver", NullValueHandling = NullValueHandling.Ignore)]
        public string RegisterDriver { get; set; }

        /// <summary>
        /// <see cref="IrbisProvider"/> to register
        /// before report building.
        /// </summary>
        [CanBeNull]
        [JsonProperty("registerProvider", NullValueHandling = NullValueHandling.Ignore)]
        public string RegisterProvider { get; set; }

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

        #region Public methods

        /// <summary>
        /// Load <see cref="ReportSettings"/>
        /// from specified file.
        /// </summary>
        public static ReportSettings LoadFromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

#if WINMOBILE

            ReportSettings result = new ReportSettings();

#else

            ReportSettings result = JsonUtility
                .ReadObjectFromFile<ReportSettings>(fileName);

#endif

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ReportSettings> verifier
                = new Verifier<ReportSettings>(this, throwOnError);

            verifier
                .NotNull(Assemblies, "Assemblies")
                .NotNullNorEmpty(DriverName)
                .NotNullNorEmpty(ProviderName);

            return verifier.Result;
        }

        #endregion
    }
}
