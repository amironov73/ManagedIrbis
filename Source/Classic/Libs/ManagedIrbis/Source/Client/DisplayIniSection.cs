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
        : AbstractIniSection
    {
        #region Constants

        /// <summary>
        /// Section name.
        /// </summary>
        public const string SectionName = "Display";

        #endregion

        #region Properties

        /// <summary>
        /// Размер порции для показа кратких описаний.
        /// </summary>
        [XmlElement("maxBriefPortion")]
        [JsonProperty("maxBriefPortion")]
        public int MaxBriefPortion
        {
            get { return Section.GetValue("MaxBriefPortion", 6); }
            set { Section.SetValue("MaxBriefPortion", value); }
        }

        /// <summary>
        /// Максимальное количество отмеченных документов.
        /// </summary>
        [XmlElement("maxMarked")]
        [JsonProperty("maxMarked")]
        public int MaxMarked
        {
            get { return Section.GetValue("MaxMarked", 100); }
            set { Section.SetValue("MaxMarked", value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisplayIniSection()
            : base(SectionName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisplayIniSection
            (
                [NotNull] IniFile iniFile
            )
            : base(iniFile, SectionName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisplayIniSection
            (
                [NotNull] IniFile.Section section
            )
            : base(section)
        {
        }

        #endregion
    }
}
