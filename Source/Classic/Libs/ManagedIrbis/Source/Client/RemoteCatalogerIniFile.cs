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
        /// Имя файла пакетного задания для АВТОВВОДА.
        /// </summary>
        public string AutoinFile
        {
            get { return GetValue(Main, "AUTOINFILE", "autoin.gbl"); }
        }

        /// <summary>
        /// Разрешает (значение 1) или запрещает (значение 0) 
        /// автоматическое слияние двух версий записи при корректировке 
        /// (при получении сообщения о несовпадении версий – в ситуации, 
        /// когда одну запись пытаются одновременно откорректировать 
        /// два и более пользователей) Автоматическое слияние проводится 
        /// по формальному алгоритму: неповторяющиеся поля заменяются, 
        /// а оригинальные значения повторяющихся полей суммируются
        /// </summary>
        public bool AutoMerge
        {
            get { return GetValue(Main, "AUTOMERGE", false); }
        }

        /// <summary>
        /// Имя краткого (строкa) формата показа.
        /// </summary>
        public string BriefPft
        {
            get { return GetValue(Main, "BRIEFPFT", "brief.pft"); }
        }

        /// <summary>
        /// Интервал в мин., по истечении которого клиент посылает 
        /// на сервер уведомление о том, что он «жив».
        /// </summary>
        public int ClientTimeLive
        {
            get { return GetValue(Main, "CLIENT_TIME_LIVE", 15); }
        }

        /// <summary>
        /// Имя файла-справочника со списком ТВП переформатирования 
        /// для копирования.
        /// </summary>
        public string CopyMnu
        {
            get { return GetValue(Main, "COPYMNU", "fst.mnu"); }
        }

        /// <summary>
        /// Метка поля «количество выдач» в БД ЭК.
        /// </summary>
        public string CountTag
        {
            get { return GetValue(Main, "DBNTAGSPROS", "999"); }
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
        /// Имя шаблона для создания новой БД.
        /// </summary>
        public string EmptyDbn
        {
            get { return GetValue(Main, "EMPTYDBN", "BLANK"); }
        }

        /// <summary>
        /// Метка поля «экземпляры» в БД ЭК.
        /// </summary>
        public string ExemplarTag
        {
            get { return GetValue(Main, "DBNTAGEKZ", "910"); }
        }

        /// <summary>
        /// Имя файла-справочника со списком ТВП переформатирования 
        /// для экспорта.
        /// </summary>
        public string ExportMenu
        {
            get { return GetValue(Main, "EXPORTMNU", "export.mnu"); }
        }

        /// <summary>
        /// Имя файла-справочника со списком доступных РЛ.
        /// </summary>
        public string FormatMenu
        {
            get { return GetValue(Main, "FMTMNU", "fmt.mnu"); }
        }

        /// <summary>
        /// Имя БД, содержащей тематический рубрикатор ГРНТИ.
        /// </summary>
        public string HelpDbn
        {
            get { return GetValue(Main, "HELPDBN", "HELP"); }
        }

        /// <summary>
        /// Имя файла-справочника со списком ТВП переформатирования 
        /// для импорта.
        /// </summary>
        public string ImportMenu
        {
            get { return GetValue(Main, "IMPORTMNU", "import.mnu"); }
        }

        /// <summary>
        /// Префикс инверсии для шифра документа в БД ЭК.
        /// </summary>
        public string IndexPrefix
        {
            get { return GetValue(Main, "DBNPREFSHIFR", "I="); }
        }

        /// <summary>
        /// Метка поля «шифр документа» в БД ЭК.
        /// </summary>
        public string IndexTag
        {
            get { return GetValue(Main, "DBNTAGSHIFR", "903"); }
        }

        /// <summary>
        /// INI-file.
        /// </summary>
        [NotNull]
        public IniFile Ini { get; private set; }

        /// <summary>
        /// Имя файла-справочника со списком постоянных запросов.
        /// </summary>
        public string IriMenu
        {
            get { return GetValue(Main, "IRIMNU", "iri.mnu"); }
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
        /// Имя файла-справочника со списком доступных форматов 
        /// показа документов.
        /// </summary>
        public string PftMenu
        {
            get { return GetValue(Main, "PFTMNU", "pft.mnu"); }
        }

        /// <summary>
        /// Имя оптимизационного файла, который определяет принцип 
        /// формата ОПТИМИЗИРОВАННЫЙ (в АРМах Читатель и Каталогизатор).
        /// Для БД электронного каталога (IBIS) значение PFTW.OPT 
        /// определяет в качестве оптимизированных  RTF-форматы, 
        /// а значение PFTW_H.OPT – HTML-форматы
        /// </summary>
        public string PftOpt
        {
            get { return GetValue(Main, "PFTOPT", "pft.opt"); }
        }

        /// <summary>
        /// Имя дополнительного INI-файла со сценариями поиска для БД.
        /// </summary>
        public string SearchIni
        {
            get { return GetValue(Main, "SEARCHINI", string.Empty); }
        }

        /// <summary>
        /// Имя эталонной БД Электронного каталога.
        /// </summary>
        public string StandardDbn
        {
            get { return GetValue(Main, "ETALONDBN", "IBIS"); }
        }

        /// <summary>
        /// Директория для сохранения временных (выходных) данных.
        /// </summary>
        public string WorkDirectory
        {
            get { return GetValue(Main, "WORKDIR", "/irbiswrk"); }
        }

        /// <summary>
        /// Имя файла оптимизации РЛ ввода.
        /// </summary>
        public string WsOpt
        {
            get { return GetValue(Main, "WSOPT", "ws.opt"); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RemoteCatalogerIniFile
            (
                [NotNull] IniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            Ini = iniFile;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get value.
        /// </summary>
        [CanBeNull]
        public string GetValue
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] string defaultValue
            )
        {
            Code.NotNullNorEmpty(sectionName, "sectionName");
            Code.NotNullNorEmpty(keyName, "keyName");

            string result = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                );

            return result;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        [CanBeNull]
        public T GetValue<T>
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] T defaultValue
            )
        {
            Code.NotNullNorEmpty(sectionName, "sectionName");
            Code.NotNullNorEmpty(keyName, "keyName");

            T result = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                );

            return result;
        }

        /// <summary>
        /// Set value.
        /// </summary>
        public RemoteCatalogerIniFile SetValue
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] string value
            )
        {
            Code.NotNullNorEmpty(sectionName, "sectionName");
            Code.NotNullNorEmpty(keyName, "keyName");

            Ini.SetValue
                (
                    sectionName,
                    keyName,
                    value
                );

            return this;
        }

        /// <summary>
        /// Set value.
        /// </summary>
        public RemoteCatalogerIniFile SetValue<T>
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] T value
            )
        {
            Code.NotNullNorEmpty(sectionName, "sectionName");
            Code.NotNullNorEmpty(keyName, "keyName");

            Ini.SetValue
                (
                    sectionName,
                    keyName,
                    value
                );

            return this;
        }

        #endregion
    }
}
