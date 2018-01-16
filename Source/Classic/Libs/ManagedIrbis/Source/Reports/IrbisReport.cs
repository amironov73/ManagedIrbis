// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisReport.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

#if ANDROID

extern alias json;

#endif

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if !WINMOBILE

using System.Runtime.Serialization.Formatters;

#endif

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class IrbisReport
        : IAttributable,
        IVerifiable,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// Attributes.
        /// </summary>
        [NotNull]
        [XmlArray("attr")]
        [JsonProperty("attr")]
        public ReportAttributes Attributes { get; private set; }

        /// <summary>
        /// Report body band.
        /// </summary>
        [NotNull]
        [XmlElement("details")]
        [JsonProperty("details")]
        public BandCollection<ReportBand> Body { get; set; }

        /// <summary>
        /// Footer band.
        /// </summary>
        [CanBeNull]
        [XmlElement("footer")]
        [JsonProperty("footer")]
        public ReportBand Footer
        {
            get { return _footer; }
            set
            {
                if (!ReferenceEquals(_footer, null))
                {
                    _footer.Report = null;
                    _footer.Parent = null;
                }
                _footer = value;
                if (!ReferenceEquals(_footer, null))
                {
                    _footer.Report = this;
                    _footer.Parent = null;
                }
            }
        }

        /// <summary>
        /// Header band.
        /// </summary>
        [CanBeNull]
        [XmlElement("header")]
        [JsonProperty("header")]
        public ReportBand Header
        {
            get { return _header; }
            set
            {
                if (!ReferenceEquals(_header, null))
                {
                    _header.Report = null;
                    _header.Parent = null;
                }
                _header = value;
                if (!ReferenceEquals(_header, null))
                {
                    _header.Report = this;
                    _header.Parent = null;
                }
            }
        }

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
            Log.Trace("IrbisReport::Constructor");

            Attributes = new ReportAttributes();
            Body = new BandCollection<ReportBand>(this, null);
        }

        #endregion

        #region Private members

        private ReportBand _footer, _header;

        #endregion

        #region Public methods

        /// <summary>
        /// Render the report.
        /// </summary>
        public virtual void Render
            (
                [NotNull] ReportContext context
            )
        {
            Code.NotNull(context, "context");

            Log.Trace("IrbisReport::Render");

            context.Output.Clear();

            ReportDriver driver = context.Driver;

            context.CurrentRecord = null;
            context.Index = -1;

            driver.BeginDocument(context, this);

            if (!ReferenceEquals(Header, null))
            {
                Header.Render(context);
            }

            Body.Render(context);

            if (!ReferenceEquals(Footer, null))
            {
                Footer.Render(context);
            }

            driver.EndDocument(context, this);
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

#if WINMOBILE || PocketPC

            return new IrbisReport();

#else

            string contents = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
#if ANDROID

                TypeNameAssemblyFormat = json::System.Runtime
                    .Serialization.Formatters
                    .FormatterAssemblyStyle.Simple

#elif NETCORE

                // TODO fix it
                // TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple

#else

                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple

#endif
            };
            IrbisReport result
                = JsonConvert.DeserializeObject<IrbisReport>
                (
                    contents,
                    settings
                );

            return result;

#endif
        }

        /// <summary>
        /// Load report from the JSON file.
        /// </summary>
        public static IrbisReport LoadShortJson
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

#if WINMOBILE

            return new IrbisReport();

#else

            string contents = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            JObject obj = JObject.Parse(contents);

            var tokens = obj.SelectTokens("$..$type");
            foreach (JToken token in tokens)
            {
                JValue val = (JValue)token;

                string typeName = val.Value.ToString();
                if (!typeName.Contains('.'))
                {
                    typeName = "ManagedIrbis.Reports."
                               + typeName
                               + ", ManagedIrbis";
                    val.Value = typeName;
                }
            }

            JsonSerializer serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Objects,

#if ANDROID

                TypeNameAssemblyFormat = json::System.Runtime
                    .Serialization.Formatters
                    .FormatterAssemblyStyle.Simple

#elif NETCORE

                // TODO fix it
                // TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple

#else

                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple

#endif
            };
            IrbisReport result = obj.ToObject<IrbisReport>
                (
                    serializer
                );

            return result;

#endif
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

#if !WINMOBILE

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects,

#if ANDROID

                TypeNameAssemblyFormat = json::System.Runtime
                    .Serialization.Formatters
                    .FormatterAssemblyStyle.Simple

#elif NETCORE

                // TODO fix it
                // TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple

#else
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple

#endif
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

#endif
        }

        /// <summary>
        /// Save the report to specified file.
        /// </summary>
        public void SaveShortJson
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

#if !WINMOBILE

            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects,

#if ANDROID

                TypeNameAssemblyFormat = json::System.Runtime
                    .Serialization.Formatters
                    .FormatterAssemblyStyle.Simple

#elif NETCORE

                // TODO fix it
                // TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple

#else

                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple

#endif
            };
            JObject obj = JObject.FromObject
                (
                    this,
                    serializer
                );

            var tokens = obj.SelectTokens("$..$type");
            foreach (JToken token in tokens)
            {
                JValue val = (JValue)token;

                Type type = Type.GetType
                    (
                        val.Value.ToString(),
                        false
                    );
                if (!ReferenceEquals(type, null))
                {
                    val.Value = type.Name;
                }
            }

            while (true)
            {
                tokens = obj.SelectTokens("$..attr");
                var attr = tokens.FirstOrDefault
                    (
                        a => a.Count() == 1
                    );
                if (ReferenceEquals(attr, null))
                {
                    break;
                }
                attr.Parent.Remove();
            }

            while (true)
            {
                tokens = obj.SelectTokens("$..cells");
                var attr = tokens.FirstOrDefault
                    (
                        a => a.Count() == 0
                    );
                if (ReferenceEquals(attr, null))
                {
                    break;
                }
                attr.Parent.Remove();
            }

            string contents = obj.ToString(Formatting.Indented);
            File.WriteAllText
                (
                    fileName,
                    contents,
                    IrbisEncoding.Utf8
                );

#endif
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<IrbisReport> verifier
                = new Verifier<IrbisReport>(this, throwOnError);

            verifier.VerifySubObject(Attributes, "attributes");

            if (!ReferenceEquals(Header, null))
            {
                verifier
                    .VerifySubObject(Header, "header")
                    .ReferenceEquals
                        (
                            Header.Parent,
                            null,
                            "Header.Parent != null"
                        )
                    .ReferenceEquals
                        (
                            Header.Report,
                            this,
                            "Header.Report != this"
                        );
            }

            if (!ReferenceEquals(Footer, null))
            {
                verifier
                    .VerifySubObject(Footer, "footer")
                    .ReferenceEquals
                        (
                            Footer.Parent,
                            null,
                            "Footer.Parent != null"
                        )
                    .ReferenceEquals
                        (
                            Footer.Report,
                            this,
                            "Footer.Report != this"
                        );
            }

            verifier.VerifySubObject(Body, "body");

            foreach (ReportBand band in Body)
            {
                verifier
                    .ReferenceEquals
                        (
                            band.Parent,
                            null,
                            "band.Parent != null"
                        )
                    .ReferenceEquals
                        (
                            band.Report,
                            this,
                            "band.Report != this"
                        );
            }

            return verifier.Result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Log.Trace("IrbisReport::Dispose");

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
