// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DisplayIniSection.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

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
    public sealed class DisplayIniSection
    {
        #region Constants

        /// <summary>
        /// Section name.
        /// </summary>
        public const string SectionName = "DISPLAY";

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
        /// Размер порции для показа кратких описаний.
        /// </summary>
        public int MaxBriefPortion
        {
            get { return Section.GetValue("MAXBRIEFPORTION", 6); }
            set { Section.SetValue("MAXBRIEFPORTION", value); }
        }

        /// <summary>
        /// Максимальное количество отмеченных документов.
        /// </summary>
        public int MaxMarked
        {
            get { return Section.GetValue("MAXMARKED", 6); }
            set { Section.SetValue("MAXMARKED", value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisplayIniSection()
        {
            IniFile iniFile = new IniFile();
            Section = iniFile.CreateSection(SectionName);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisplayIniSection
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
        public DisplayIniSection
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

        /// <summary>
        /// Clear the section.
        /// </summary>
        [NotNull]
        public DisplayIniSection Clear()
        {
            Section.Clear();

            return this;
        }

        #endregion

        #region Object members

        #endregion
    }
}
