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
    /// Section of client INI-file.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class AbstractIniSection
    {
        #region Properties

        /// <summary>
        /// INI file section.
        /// </summary>
        [NotNull]
        [XmlIgnore]
        [JsonIgnore]
        public IniFile.Section Section { get; protected set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbstractIniSection
            (
                [NotNull] IniFile iniFile,
                [NotNull] string sectionName
            )
        {
            Code.NotNull(iniFile, "iniFile");
            Code.NotNullNorEmpty(sectionName, "sectionName");

            Section = iniFile.GetOrCreateSection(sectionName);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbstractIniSection
            (
                [NotNull] IniFile.Section section
            )
        {
            Code.NotNull(section, "section");

            Section = section;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the section.
        /// </summary>
        public void Clear()
        {
            Section.Clear();
        }

        /// <summary>
        /// Get boolean value
        /// </summary>
        public bool GetBoolean
            (
                [NotNull] string name,
                [NotNull] string defaultValue
            )
        {
            Code.NotNullNorEmpty(name, "name");
            Code.NotNullNorEmpty(defaultValue, "defaultValue");

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

        /// <summary>
        /// Set boolean value.
        /// </summary>
        public void SetBoolean
            (
                [NotNull] string name,
                bool value
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Section.SetValue
                (
                    name,
                    value ? "1" : "0"
                );
        }

        #endregion
    }
}
