// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EntryIniSection.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Находится в серверном INI-файле irbisc.ini.
    /// </remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class EntryIniSection
    {
        #region Constants

        /// <summary>
        /// Section name.
        /// </summary>
        public const string SectionName = "CONTEXT";

        #endregion

        #region Properties

        /// <summary>
        /// INI file section.
        /// </summary>
        [NotNull]
        [XmlIgnore]
        [JsonIgnore]
        public IniFile.Section Section { get; private set; }

        // ========================================================

        /// <summary>
        /// Имя формата для ФЛК документа в целом.
        /// </summary>
        [CanBeNull]
        [XmlElement("dbnflc")]
        [JsonProperty("dbnflc")]
        public string DbnFlc
        {
            get { return Section["DBNFLC"]; }
            set { Section["DBNFLC"] = value; }
        }

        /// <summary>
        /// Признак автоматической актуализации записей
        /// при корректировке.
        /// </summary>
        public bool RecordUpdate
        {
            get
            {
                return ConversionUtility.ToBoolean
                    (
                        Section.GetValue("RECUPDIF", "1")
                            .ThrowIfNull()
                    );
            }
            set
            {
                Section.SetValue("RECUPDIF", value ? "1" : "0");
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntryIniSection()
        {
            IniFile iniFile = new IniFile();
            Section = iniFile.CreateSection(SectionName);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntryIniSection
        (
            [NotNull] IniFile iniFile
        )
        {
            Code.NotNull(iniFile, "iniFile");

            Section = iniFile.GetOrCreateSection(SectionName);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntryIniSection
        (
            [NotNull] IniFile.Section section
        )
        {
            Code.NotNull(section, "section");

            Section = section;
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
