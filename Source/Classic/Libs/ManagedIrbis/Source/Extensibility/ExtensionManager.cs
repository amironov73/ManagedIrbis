// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExtensionManager.cs -- 
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
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace ManagedIrbis.Extensibility
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ExtensionManager
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const string USERMODE = "USERMODE";

        /// <summary>
        /// 
        /// </summary>
        public const string UMNUMB = "UMNUMB";

        #endregion

        #region Properties

        /// <summary>
        /// Extension list.
        /// </summary>
        [NotNull]
        public NonNullCollection<ExtensionInfo> Extensions { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExtensionManager()
        {
            Extensions = new NonNullCollection<ExtensionInfo>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Build extension list from the INI-file.
        /// </summary>
        [NotNull]
        public static ExtensionManager FromIniFile
            (
                [NotNull] IniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            IniFile.Section section = iniFile.GetSection(USERMODE)
                .ThrowIfNull(USERMODE);
            int count = section.GetValue("UMNUMB", 0);
            ExtensionManager result = new ExtensionManager();
            for (int index = 0; index < count; index++)
            {
                ExtensionInfo item = ExtensionInfo.FromIniFile
                    (
                        section,
                        index
                    );
                result.Extensions.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Update the INI-file.
        /// </summary>
        public void UpdateIniFile
            (
                [NotNull] IniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            IniFile.Section section = iniFile
                .GetOrCreateSection(USERMODE)
                .ThrowIfNull(USERMODE);
            section.Clear();
            section.SetValue(UMNUMB, Extensions.Count);

            for (int index = 0; index < Extensions.Count; index++)
            {
                ExtensionInfo item = Extensions[index];
                item.Index = index;
                item.UpdateIniFile(section);
            }
        }

        #endregion
    }
}
