// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisLink.cs -- 
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
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    //
    // http://sntnarciss.ru/irbis/spravka/irbishelp.html?wc00202090100.htm
    //

    //
    // ИРБИС-ссылка является «надстройкой» HTML для реализации функций ИРБИС-Навигатора.
    // В общем виде ИРБИС-ссылка имеет следующий вид:
    // IRBIS:[параметры_ссылки]
    //
    // ИРБИС-ссылка может использоваться и формироваться в HTML-странице ТОЧНО так же, как и URL-ссылка.
    // Все параметры ИРБИС-ссылки могут передаваться методом GET
    // (т.е. непосредственно в ссылке после символа ?) или методом POST.
    // Для обеспечения преемственности по отношению к ссылкам,
    // применяемым в стандартных форматах для других АРМов ИРБИС,
    // часть параметров может передаваться позиционно
    // в основной части ссылки (до символа ?).
    // Т.е. в общем виде ИРБИС-ссылка может быть представлена как:
    //
    //  IRBIS:[позиционные_параметры]?[поименованные параметры]
    //
    // Каждая ИРБИС-ссылка реализует ОДНУ команду, поэтому среди передаваемых
    // параметров ОБЯЗАТЕЛЬНО должен присутствовать параметр,
    // идентифицирующий команду, - имя этого параметра по умолчанию C21COM.
    // 
    // Поименованные параметры передаются в соответствии с правилами и структурой URL.
    //
    // Позиционные параметры
    //
    // В качестве позиционных параметров могут передаваться ТОЛЬКО следующие параметры:
    // * идентификатор команды (C21COM);
    // * имя БД (I21DBN);
    // * имя формата (PFTNAME);
    // * ключ (KEY);
    // * путь (PATH);
    // *имя файлового ресурса (FILENAME).
    //
    // Для всех команд, кроме 3*, структура позиционных параметров имеет вид:
    //
    // N,,dbname,pftname,key
    //
    // где N – идентификатор команды.
    //
    // Для команды 3* структура позиционных параметров имеет вид:
    //
    // 3,path,dbname,filename
    //
    // Повторим, что наличие и структура позиционных параметров
    // связаны исключительно с обеспечением преемственности
    // по отношению к ссылкам, применяемым в «обычных» форматах ИРБИС.
    // Тем, для кого затруднительно понимание позиционных параметров,
    // можно рекомендовать их вообще не использовать,
    // а применять только поименованные параметры.
    //
    // Примеры ИРБИС-ссылок:
    //
    // IRBIS:1,,IBIS,FULLW0_WN,@6
    //
    // Та же самая ссылка с использованием поименованных параметров:
    //
    // IRBIS:?C21COM=1&I21DBN=IBIS&PFTNAME=FULLW0_WN&KEY=@6
    //
    // То же самое со смешанным использованием позиционных и поименованных параметров:
    //
    // IRBIS:1,,IBIS,,@6?PFTNAME=FULLW0_WN
    //
    // Внимание:
    //
    // Необходимо помнить, что при указании значений поименованных
    // параметров можно использовать только  латиницу, цифры
    // и некоторые специальные символы – для передачи остальных
    // символов используется специальное представление (для этого
    // в языке форматирования введен специальный форматный выход
    //
    // &unifor(‘+3E..’)).
    //

    // Помимо собственно ИРБИС-ссылок, работа с которыми является
    // ГЛАВНОЙ функцией ИРБИС-Навигатора, предлагаются дополнительные
    // оригинальные (т.е. интерпретируемые ТОЛЬКО ИРБИС-Навигатором)
    // конструкции HTML.
    //
    // 1. Включение в HTML-страницу графических данных
    // на основе их относительной адресации в системе ИРБИС.
    // <IMG SRC=”IRBIS:path,dbname,filename” …..>
    //
    // где:   path – относительный путь в системе ИРБИС. Принимает значения:
    // * 0 – основная директория ИРБИС;
    // * 1 – общая директория БД (.\DATAI);
    // * 2,3,10 – директория БД.
    //
    // dbname – имя БД (имеет смысл, если path принимает значения 2,3,10).
    // filename – имя графического файла.
    //
    // Пример ...<img src="irbis:0,,IRBIS.GIF">
    //

    // ====================================================================

    // Параметры ИРБИС-ссылки
    //
    // Ниже приведён список команд  (значений параметра C21COM),
    // содержащих, в свою очередь, ряд параметров ИРБИС-ссылки:
    //
    // Команда (значение параметра C21COM) Параметры команды
    //
    // (любая – т.е. соответствующие параметры могут использоваться для любой команды)
    // BOTTOMPFTNAME, CHECKNAME, CHECKPFT, DBNAME_DEFAULT, I21DBN,
    // PFTNAME, PFTNAME_DEFAULT, RELOAD, SERVERIP, SERVERPORT, TOPPFTNAME
    // 1  Просмотр одного документа (Ссылка на один документ)
    // ENTRY_MFN, KEY
    //
    // 2  Поиск/Просмотр (Ссылка на группу документов)
    // S21STR, ***S21STR, ***S21P01, ***S21P02, ***S21P03, ***S21P04,
    // ***S21P05, ***S21LOG, S21LOG, S21STN, S21CNR, S21ALL, QUERY, S21ALLTRM, KEY
    //
    // 3  Ссылка на файловый ресурс через относительную адресацию ИРБИС
    // PATH, FILENAME
    //
    // 4  Просмотр словаря (Ссылка на фрагмент/порцию словаря)
    // T21TRM, T21PRF, T21CNR, T21NEXT, T21PREV
    //
    // 5  Просмотр списка/справочника (Ссылка на список / справочник)
    // LISTNAME, PATH
    //
    // 6  Редактирование/ввод документа (записи)
    // ENTRY_MFN,  tag_nnR21STR,  R21UPD, ENTRY_DBNFLC, R21IFP,
    // ENTRY_MESSAGE, ENTRY_AFTERACTION
    //
    // 7  Экспорт документов
    // EXPORT_FROMMFN, EXPORT_TOMFN, S21ALL, EXP21FMT, EXP21CODE,
    // EXP21FST, EXPFULLTEXT
    //
    // 8  Импорт документов
    // IMP21FMT, IMP21CODE, IMP21FST, R21IFP, ENTRY_DBNFLC
    //
    // 10  Полнотекстовый поиск
    // S21STR, S21P03, S21STN, S21CNR, S21ALL, FullText_Morphology,
    // FullText_MaxWordsDistance, FullText_MaxCountResult
    //
    // 11  Выполнение функции пользователя
    // любая
    // USERDLLNAME, USERFUNCTIONNAME, USERDATA
    //
    // (любая) – т.е. соответствующие параметры могут использоваться для любой команды.
    // FREEPARnn
    //

    //
    // Examples:
    // IRBIS:1,,IBIS,FULLW0_WN,@6
    // IRBIS:?C21COM=1&I21DBN=IBIS&PFTNAME=FULLW0_WN&KEY=@6
    // IRBIS:1,,IBIS,,@6?PFTNAME=FULLW0_WN
    //
    //
    //
    //
    // '<img src="IRBIS:!!',v910^h,'!!" align="top" height="40px">'
    // '<A HREF="IRBIS:1,,,bibl390_H_Full?&KEY=@',mfn,'">'
    // '<img src="IRBIS:10,,textfolder.gif">'
    // '<A HREF=IRBIS:?C21COM=1&KEY=@',mfn,'&I21DBN=',&uf('+D'),'&PFTNAME=fullw0_wn> Полностью...</A>'
    // '<A HREF="IRBIS:?C21COM=1&I21DBN=',&uf('+D'),'&KEY=@',mfn,'&PFTNAME=rqst_IC_wn&FREEPAR3=',g1103,'&FREEPAR0=',g1100,'&FREEPAR5=',g1105,'">Заказать...</A>',
    // '<b>','<IRBIS type=0>. ','</b>'
    // '<A HREF="IRBIS:2,,,I='v430^d'">',v430^a,'</A>'
    // '<a href="IRBIS:5,,,MNU_WN?FREEPAR0=',v1100,'">Отобранные</a>'
    // '<IMG SRC="IRBIS:12,',&uf('+D'),',',mfn,',1">'
    // '<A HREF="IRBIS:3,12,,',mfn,',','1','">','<IMG  style="width:105 px" SRC="IRBIS:12,,',mfn,',','1','"></A>'
    // (if p(v951)then if v951^h:'02a'and v951^a<>''then '<A HREF="IRBIS:3?PATH=11&FILENAME=',v951^a,'">','<IMG  style="width:150 px" SRC="IRBIS:11,,',v951^a,'"></A>'BREAK,fi,fi)
    //

    //
    // WEB FORM:
    //
    // '<form name="CONTINUE1" action="IRBIS:" enctype="Multipart/form-data" method="GET">',
    // '<input id="anchor" name="T21TRM" value="',&uf('AV1004^A#1')'" onkeydown="if (event.keyCode==13) {CONTINUE1.submit();}">',
    // ' <input type="submit" name="" value="Продолжить" id="anchor" >',
    // '<IMG SRC="IRBIS:10,,arrow_up.jpg">',
    // '<input type="hidden" name="T21NEXT" value="0">',
    // '<input type="hidden" name="C21COM" value="4">',
    // '<input type="hidden" name="I21DBN" value="',v1000,'">',
    // '<input type="hidden" name="PFTNAME" value="',v1012,'">',
    // '<input type="hidden" name="T21PRF" value="',v1011,'">',
    // '<input type="hidden" name="T21CNR" value="',v1006,'">',
    // '<input type="hidden" name="FREEPAR0" value="',v1100,'">',
    // '<input type="hidden" name="FREEPAR1" value="',v1101,'">',
    // '</form>'
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisLink
    {
        #region Properties

        /// <summary>
        /// Command (C21COM).
        /// </summary>
        /// <remarks>
        /// E. g.: "3".
        /// </remarks>
        [CanBeNull]
        public string Command { get; set; }

        /// <summary>
        /// Path (PATH).
        /// </summary>
        /// <remarks>
        /// E. g.: "10".
        /// </remarks>
        [CanBeNull]
        public string Path { get; set; }

        /// <summary>
        /// Database name (I21DBN).
        /// </summary>
        /// <remarks>
        /// E. g.: "IBIS".
        /// </remarks>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// PFT file name (PFTNAME).
        /// </summary>
        [CanBeNull]
        public string Format { get; set; }

        /// <summary>
        /// File name (FILENAME).
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        /// <summary>
        /// Search key (KEY).
        /// </summary>
        [CanBeNull]
        public string Key { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        [NotNull]
        public Dictionary<string, string> Parameters { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisLink()
        {
            Parameters = new Dictionary<string, string>();
        }

        #endregion

        #region Private members

        private static string _GetPositional
            (
                TextNavigator navigator
            )
        {
            string result = navigator.ReadUntil(',', '?');
            if (navigator.PeekChar() == ',')
            {
                navigator.ReadChar();
            }

            return result;
        }

        private static bool _GetNamed
            (
                TextNavigator navigator,
                Dictionary<string, string> dictionary
            )
        {
            string key = navigator.ReadUntil('=');
            if (string.IsNullOrEmpty(key)
                || navigator.ReadChar() != '=')
            {
                return false;
            }

            string value = navigator.ReadUntil('&');
            dictionary.Add(key, value);

            return navigator.ReadChar() == '&';
        }

        private static string _SetPositional
            (
                Dictionary<string,string> dictionary,
                string key,
                string value
            )
        {
            if (dictionary.ContainsKey(key))
            {
                value = dictionary[key];
            }

            return value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse text for IRBIS-link.
        /// </summary>
        [NotNull]
        public static IrbisLink Parse
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            IrbisLink result = new IrbisLink();
            TextNavigator navigator = new TextNavigator(text);
            string prefix = navigator.PeekString(6);
            if (prefix.SameString("IRBIS:"))
            {
                navigator.ReadString(6);
            }

            result.Command = _GetPositional(navigator);
            if (result.Command.SameString("3"))
            {
                result.Path = _GetPositional(navigator);
                result.Database = _GetPositional(navigator);
                result.FileName = _GetPositional(navigator);
            }
            else
            {
                _GetPositional(navigator);
                result.Database = _GetPositional(navigator);
                result.Format = _GetPositional(navigator);
                result.Key = _GetPositional(navigator);
            }

            if (navigator.ReadChar() == '?')
            {
                while (_GetNamed(navigator, result.Parameters))
                {
                    // Do nothing
                }

                result.Command = _SetPositional(result.Parameters, "C21COM", result.Command);
                result.Database = _SetPositional(result.Parameters, "I21DBN", result.Database);
                result.Format = _SetPositional(result.Parameters, "PFTNAME", result.Format);
                result.Key = _SetPositional(result.Parameters, "KEY", result.Key);
                result.Path = _SetPositional(result.Parameters, "PATH", result.Path);
                result.FileName = _SetPositional(result.Parameters, "FILENAME", result.FileName);
            }

            return result;
        }

        /// <summary>
        /// Parse text for IRBIS-link.
        /// </summary>
        [NotNull]
        public static IrbisLink ParseForImage
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            IrbisLink result = new IrbisLink();
            TextNavigator navigator = new TextNavigator(text);
            string prefix = navigator.PeekString(6);
            if (prefix.SameString("IRBIS:"))
            {
                navigator.ReadString(6);
            }

            result.Command = "3";
            result.Path = _GetPositional(navigator);
            result.Database = _GetPositional(navigator).EmptyToNull();
            result.FileName = _GetPositional(navigator);

            if (navigator.ReadChar() == '?')
            {
                while (_GetNamed(navigator, result.Parameters))
                {
                    // Do nothing
                }

                result.Database = _SetPositional(result.Parameters, "I21DBN", result.Database);
                result.Format = _SetPositional(result.Parameters, "PFTNAME", result.Format);
                result.Key = _SetPositional(result.Parameters, "KEY", result.Key);
                result.Path = _SetPositional(result.Parameters, "PATH", result.Path);
                result.FileName = _SetPositional(result.Parameters, "FILENAME", result.FileName);
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format
                (
                    "Command: {0}",
                    Command.ToVisibleString()
                );
        }

        #endregion
    }
}
