/* LocalCatalogerIniFile.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// INI-file for cataloger.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class LocalCatalogerIniFile
        : IniFile
    {
        #region Constants

        /// <summary>
        /// Main section.
        /// </summary>
        public const string Main = "Main";

        #endregion

        #region Properties
        
        /// <summary>
        /// Main section.
        /// </summary>
        [NotNull]
        public Section MainSection
        {
            get { return GetSection(Main).ThrowIfNull(); }
        }

        /// <summary>
        /// Организация, на которую куплен ИРБИС.
        /// </summary>
        public string Organization
        {
            get { return MainSection["User"]; }
        }

        /// <summary>
        /// IP адрес ИРБИС сервера.
        /// </summary>
        [NotNull]
        public string ServerIP
        {
            get { return MainSection["ServerIP"]; }
        }

        /// <summary>
        /// Номер порта ИРБИС сервера.
        /// </summary>
        public int ServerPort
        {
            get
            {
                int result = Convert.ToInt32
                    (
                        MainSection["ServerPort"]
                    );

                return result;
            }
        }

        #endregion

        #region Public methods

        #endregion
    }
}
