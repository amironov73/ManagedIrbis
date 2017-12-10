// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ContextIniSection.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM.IO;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ContextIniSection
        : AbstractIniSection
    {
        #region Constants

        /// <summary>
        /// Section name.
        /// </summary>
        public const string SectionName = "CONTEXT";

        #endregion

        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        [XmlElement("database")]
        [JsonProperty("database")]
        public string Database
        {
            get { return Section["DBN"]; }
            set { Section["DBN"] = value; }
        }

        /// <summary>
        /// Display format description.
        /// </summary>
        [CanBeNull]
        [XmlElement("format")]
        [JsonProperty("format")]
        public string DisplayFormat
        {
            get { return Section["PFT"]; }
            set { Section["PFT"] = value; }
        }

        /// <summary>
        /// Current MFN.
        /// </summary>
        [XmlElement("mfn")]
        [JsonProperty("mfn")]
        public int Mfn
        {
            get { return Section.GetValue("CURMFN", 0); }
            set { Section.SetValue("CURMFN", value); }
        }

        /// <summary>
        /// Password.
        /// </summary>
        [CanBeNull]
        [XmlElement("password")]
        [JsonProperty("password")]
        public string Password
        {
            get { return Section["UserPassword"]; }
            set { Section["UserPassword"] = value; }
        }

        /// <summary>
        /// Query.
        /// </summary>
        [CanBeNull]
        [XmlElement("query")]
        [JsonProperty("query")]
        public string Query
        {
            // TODO использовать UTF8

            get { return Section["QUERY"]; }
            set { Section["QUERY"] = value; }
        }

        /// <summary>
        /// Search prefix.
        /// </summary>
        [CanBeNull]
        [XmlElement("prefix")]
        [JsonProperty("prefix")]
        public string SearchPrefix
        {
            get { return Section["PREFIX"]; }
            set { Section["PREFIX"] = value; }
        }

        /// <summary>
        /// User name.
        /// </summary>
        [CanBeNull]
        [XmlElement("username")]
        [JsonProperty("username")]
        public string UserName
        {
            get { return Section["UserName"]; }
            set { Section["UserName"] = value; }
        }

        /// <summary>
        /// Worksheet code.
        /// </summary>
        [CanBeNull]
        [XmlElement("worksheet")]
        [JsonProperty("worksheet")]
        public string Worksheet
        {
            get { return Section["WS"]; }
            set { Section["WS"] = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ContextIniSection()
            : base (SectionName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ContextIniSection
            (
                [NotNull] IniFile iniFile
            )
            : base(iniFile, SectionName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ContextIniSection
            (
                [NotNull] IniFile.Section section
            )
            : base(section)
        {
        }

        #endregion
    }
}
