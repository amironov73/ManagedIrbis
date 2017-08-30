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
            : base(SectionName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntryIniSection
            (
                [NotNull] IniFile iniFile
            )
            : base(iniFile, SectionName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntryIniSection
            (
                [NotNull] IniFile.Section section
            )
            : base(section)
        {
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
