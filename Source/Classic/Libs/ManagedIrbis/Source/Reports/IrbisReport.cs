// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisReport.cs -- 
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
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

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
    public class IrbisReport
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Report body band.
        /// </summary>
        [NotNull]
        [XmlElement("details")]
        [JsonProperty("details")]
        public BandCollection<DetailsBand> Body { get; set; }

        /// <summary>
        /// Footer band.
        /// </summary>
        [CanBeNull]
        [XmlElement("footer")]
        [JsonProperty("footer")]
        public ReportBand Footer { get; set; }

        /// <summary>
        /// Header band.
        /// </summary>
        [CanBeNull]
        [XmlElement("header")]
        [JsonProperty("header")]
        public ReportBand Header { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisReport()
        {
            Body = new BandCollection<DetailsBand>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Render the report.
        /// </summary>
        public virtual void Evaluate
            (
                [NotNull] ReportContext context
            )
        {
            Code.NotNull(context, "context");

            context.Output.Clear();

            ReportDriver driver = context.Driver;

            driver.BeginDocument(context);

            if (!ReferenceEquals(Header, null))
            {
                Header.Evaluate(context);
            }

            foreach (DetailsBand band in Body)
            {
                band.Evaluate(context);
            }

            if (!ReferenceEquals(Footer, null))
            {
                Footer.Evaluate(context);
            }

            driver.EndDocument(context);
        }

        /// <summary>
        /// Load report from the JSON file.
        /// </summary>
        [NotNull]
        public static IrbisReport LoadJsonFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string contents = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            };
            IrbisReport result
                = JsonConvert.DeserializeObject<IrbisReport>
                (
                    contents,
                    settings
                );

            return result;
        }

        /// <summary>
        /// Save the report to specified file.
        /// </summary>
        public void SaveJson
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            };
            string contents = JsonConvert.SerializeObject
                (
                    this,
                    Formatting.Indented,
                    settings
                );
            File.WriteAllText
                (
                    fileName,
                    contents,
                    IrbisEncoding.Utf8
                );
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (!ReferenceEquals(Header, null))
            {
                Header.Dispose();
            }
            if (!ReferenceEquals(Footer, null))
            {
                Footer.Dispose();
            }
            Body.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
