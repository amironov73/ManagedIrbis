// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DesktopIniSection.cs -- 
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
    /// Находится в клиентском INI-файле cirbisc.ini.
    /// </remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DesktopIniSection
    {
        #region Constants

        /// <summary>
        /// Section name.
        /// </summary>
        public const string SectionName = "DESKTOP";

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
        /// Use auto service?
        /// </summary>
        [XmlElement("autoService")]
        [JsonProperty("autoService")]
        public bool AutoService
        {
            get { return GetBoolean("AutoService", "1"); }
            set { SetBoolean("AutoService", value);}
        }

        /// <summary>
        /// Show database context panel?
        /// </summary>
        [XmlElement("dbContext")]
        [JsonProperty("dbContext")]
        public bool DBContext
        {
            get { return GetBoolean("DBContext", "1"); }
            set { SetBoolean("DBContext", value); }
        }

        /// <summary>
        /// Database context panel is floating?
        /// </summary>
        [XmlElement("dbContextFloating")]
        [JsonProperty("dbContextFloating")]
        public bool DBContextFloating
        {
            get { return GetBoolean("DBContextFloating", "0"); }
            set { SetBoolean("DBContextFloating", value); }
        }

        /// <summary>
        /// DBOpen panel visible?
        /// </summary>
        [XmlElement("dbOpen")]
        [JsonProperty("dbOpen")]
        public bool DBOpen
        {
            get { return GetBoolean("DBOpen", "1"); }
            set { SetBoolean("DBOpen", value); }
        }

        /// <summary>
        /// Whether DBOpen panel is floating?
        /// </summary>
        [XmlElement("dbOpenFloating")]
        [JsonProperty("dbOpenFloating")]
        public bool DBOpenFloating
        {
            get { return GetBoolean("DBOpenFloating", "0"); }
            set { SetBoolean("DBOpenFloating", value); }
        }

        /// <summary>
        /// Show the entry panel?
        /// </summary>
        [XmlElement("entry")]
        [JsonProperty("entry")]
        public bool Entry
        {
            get { return GetBoolean("Entry", "1"); }
            set { SetBoolean("Entry", value); }
        }

        /// <summary>
        /// Entry panel is floating?
        /// </summary>
        [XmlElement("entryFloating")]
        [JsonProperty("entryFloating")]
        public bool EntryFloating
        {
            get { return GetBoolean("EntryFloating", "0"); }
            set { SetBoolean("EntryFloating", value); }
        }

        /// <summary>
        /// Show then main menu?
        /// </summary>
        [XmlElement("mainMenu")]
        [JsonProperty("mainMenu")]
        public bool MainMenu
        {
            get { return GetBoolean("MainMenu", "1"); }
            set { SetBoolean("MainMenu", value); }
        }

        /// <summary>
        /// Main menu is floating panel?
        /// </summary>
        [XmlElement("mainMenuFloating")]
        [JsonProperty("mainMenuFloating")]
        public bool MainMenuFloating
        {
            get { return GetBoolean("MainMenuFloating", "0"); }
            set { SetBoolean("MainMenuFloating", value); }
        }

        /// <summary>
        /// Show the search panel?
        /// </summary>
        [XmlElement("search")]
        [JsonProperty("search")]
        public bool Search
        {
            get { return GetBoolean("Search", "1"); }
            set { SetBoolean("Search", value); }
        }

        /// <summary>
        /// Whether the search panel is floating?
        /// </summary>
        [XmlElement("searchFloating")]
        [JsonProperty("searchFloating")]
        public bool SearchFloating
        {
            get { return GetBoolean("SearchFloating", "0"); }
            set { SetBoolean("SearchFloating", value); }
        }

        /// <summary>
        /// Use spelling engine?
        /// </summary>
        [XmlElement("spelling")]
        [JsonProperty("spelling")]
        public bool Spelling
        {
            get { return GetBoolean("Spelling", "1"); }
            set { SetBoolean("Spelling", value); }
        }

        /// <summary>
        /// Show the user mode panel?
        /// </summary>
        [XmlElement("userMode")]
        [JsonProperty("userMode")]
        public bool UserMode
        {
            get { return GetBoolean("UserMode", "1"); }
            set { SetBoolean("UserMode", value); }
        }

        /// <summary>
        /// Whether the user mode panel is floating?
        /// </summary>
        [XmlElement("userModeFloating")]
        [JsonProperty("userModeFloating")]
        public bool UserModeFloating
        {
            get { return GetBoolean("UserModeFloating", "0"); }
            set { SetBoolean("UserModeFloating", value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DesktopIniSection()
        {
            IniFile iniFile = new IniFile();
            Section = iniFile.CreateSection(SectionName);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DesktopIniSection
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
        public DesktopIniSection
        (
            [NotNull] IniFile.Section section
        )
        {
            Code.NotNull(section, "section");

            Section = section;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Get boolean value
        /// </summary>
        private bool GetBoolean
            (
                [NotNull] string name,
                string defaultValue
            )
        {
            return ConversionUtility.ToBoolean
                (
                    Section.GetValue
                        (
                            name,
                            defaultValue
                        )
                    .ThrowIfNull()
                );
        }

        private void SetBoolean
            (
                [NotNull] string name,
                bool value
            )
        {
            Section.SetValue
                (
                    name,
                    value ? "1" : "0"
                );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the section.
        /// </summary>
        [NotNull]
        public DesktopIniSection Clear()
        {
            Section.Clear();

            return this;
        }

        #endregion

        #region Object members

        #endregion
    }
}
