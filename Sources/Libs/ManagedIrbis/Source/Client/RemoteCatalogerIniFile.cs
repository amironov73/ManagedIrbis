/* RemoteCatalogerIniFile.cs --
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
    /// <summary>
    /// INI-file for cataloger.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class RemoteCatalogerIniFile
        : IniFile
    {
        #region Constants

        /// <summary>
        /// Display section name.
        /// </summary>
        public string Display = "Display";

        /// <summary>
        /// Entry section name.
        /// </summary>
        public string Entry = "Entry";

        /// <summary>
        /// Main section name.
        /// </summary>
        public string Main = "Main";

        /// <summary>
        /// Private section name.
        /// </summary>
        public string Private = "Private";

        #endregion

        #region Properties

        /// <summary>
        /// Имя краткого (строкa) формата показа.
        /// </summary>
        public string BriefPft
        {
            get { return GetValue(Main, "BRIEFPFT", "brief.pft"); }
        }

        /// <summary>
        /// Имя файла списка БД для АРМа Каталогизатора/Комплектатора.
        /// </summary>
        public string DatabaseList
        {
            get { return GetValue(Main, "DBNNAMECAT", "dbnam2.mnu"); }
        }

        /// <summary>
        /// Имя формата для ФЛК документа в целом.
        /// </summary>
        public string DbnFlc
        {
            get { return GetValue(Entry, "DBNFLC", "dbnflc.pft"); }
        }

        /// <summary>
        /// Имя базы данных по умолчанию.
        /// </summary>
        public string DefaultDb
        {
            get { return GetValue(Main, "DEFAULTDB", "IBIS"); }
        }

        /// <summary>
        /// Размер порции для показа кратких описаний.
        /// </summary>
        public int MaxBriefPortion
        {
            get { return GetValue (Main, "MAXBRIEFPORTION", 10); }
        }

        /// <summary>
        /// Максимальное количество отмеченных документов.
        /// </summary>
        public int MaxMarked
        {
            get { return GetValue(Main, "MAXMARKED", 10); }
        }

        /// <summary>
        /// Имя дополнительного INI-файла со сценариями поиска для БД.
        /// </summary>
        public string SearchIni
        {
            get { return GetValue(Main, "SEARCHINI", "search.ini"); }
        }

        #endregion

        #region Private members

        #endregion
    }
}
